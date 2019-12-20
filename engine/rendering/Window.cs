using System;

using glfw3;
using OpenTK;
using OpenTK.Graphics;

using System.Runtime.InteropServices;

using engine.input;
using engine.input.openGL;

namespace engine.rendering
{
    public class GlfwWinInfo : OpenTK.Platform.IWindowInfo
    {
        GLFWwindow glfwWindowPtr;
        IntPtr glfwHandle;

        public GlfwWinInfo(GLFWwindow glfwWindowPtr)
        {
            this.glfwWindowPtr = glfwWindowPtr;
            this.glfwHandle = glfwWindowPtr.__Instance;
        }

        public IntPtr Handle { get { return glfwHandle; } }

        public void Dispose()
        {
            Glfw.DestroyWindow(glfwWindowPtr);
        }

        internal GLFWwindow GLFWwindow { get { return glfwWindowPtr; } }
    }

    public class Window
    {
        public static IntPtr GetProcAddress(string pname)
        {
            GLFWglproc proc = Glfw.GetProcAddress(pname);

            return proc == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(proc);
        }

        public static ContextHandle GetCurrentContext()
        {
            return new ContextHandle(Glfw.GetCurrentContext().__Instance);
        }

        private GLFWwindow window;
        private static int width;
        private static int height;
        private IInput input;

        public Window(int width, int height, string title) : this(width, height, title, false) {}

        public Window(int width, int height, string title, bool isFullscreen)
        {
            if (Glfw.Init() == 0)
            {
                Console.WriteLine("Could not initialize GLFW!");
                Environment.Exit(-1);
            }

            GLFWvidmode vid = Glfw.GetVideoMode(Glfw.GetPrimaryMonitor());

            Glfw.DefaultWindowHints();
            this.window = Glfw.CreateWindow(isFullscreen ? vid.Width : width, isFullscreen ? vid.Height : height, title, 
                isFullscreen ? Glfw.GetPrimaryMonitor() : null, null);
            
            Glfw.SetWindowPos(window, (vid.Width - width) / 2, (vid.Height - height) / 2);

            if(!isFullscreen)
            {
                Window.width = width;
                Window.height = height;
            }
            else
            {
                Window.width = vid.Width;
                Window.height = vid.Height;
            }

            Glfw.MakeContextCurrent(window);
            Glfw.ShowWindow(window);
            Glfw.SwapInterval(0);

            initializeOTK();

            input = new OpenGLInput(window, width, height);
        }

        private void initializeOTK()
        {
            Toolkit.Init();

            GraphicsContext.GetAddressDelegate GetProcAddressFunc = new GraphicsContext.GetAddressDelegate(GetProcAddress);
            GraphicsContext.GetCurrentContextDelegate getCurrentContextFunc = new GraphicsContext.GetCurrentContextDelegate(GetCurrentContext);

            GlfwWinInfo glfwWinInfo = new GlfwWinInfo(window);
            IGraphicsContext context = new GraphicsContext(new ContextHandle(window.__Instance), GetProcAddressFunc, getCurrentContextFunc);
            context.MakeCurrent(glfwWinInfo);
            context.LoadAll();
        }

        public bool shouldClose()
        {
            return Glfw.WindowShouldClose(window) == 1;
        }

        public void update()
        {
            input.update();
            Glfw.PollEvents();
        }

        public void present()
        {
            Glfw.SwapBuffers(window);
        }

        public void destroy()
        {
            Glfw.DestroyWindow(window);
            Glfw.Terminate();
        }

        public static float getAR()
        {
            return (float)width / (float)height;
        }

        public static int getWidth()
        {
            return width;
        }

        public static int getHeight()
        {
            return height;
        }

        public IInput getInput()
        {
            return input;
        }
    }
}
