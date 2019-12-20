using System;
using System.Collections.Generic;
using engine.math;

using glfw3;

namespace engine.input.openGL
{
    public class OpenGLInput : IInput
    {
        private GLFWwindow inputSource;
        private double mouseX;
        private double mouseY;
        private Vector2 mousePosition;
        private Dictionary<Joystick, List<float>> joystickAxes;
        private Dictionary<Joystick, List<byte>> joystickButtons;
        private Vector2 mouseDelta;
        private bool hasBeenUpdated;

        private double halfWidth;
        private double halfHeight;

        public OpenGLInput(GLFWwindow window, int windowWidth, int windowHeight)
        {
            inputSource = window;
            mousePosition = new Vector2(0, 0);
            joystickAxes = new Dictionary<Joystick, List<float>>();
            joystickButtons = new Dictionary<Joystick, List<byte>>();
            mouseDelta = new Vector2(0, 0);
            hasBeenUpdated = false;
            this.halfWidth = (double)windowWidth / 2.0;
            this.halfHeight = (double)windowHeight / 2.0;
        }

        private void initJoystick(Joystick joystick)
        {
            if (!joystickAxes.ContainsKey(joystick))
            {
                joystickAxes.Add(joystick, new List<float>());
                joystickButtons.Add(joystick, new List<byte>());
                updateJoystick(joystick);
            }
        }

        private unsafe float[] convert(float* arr, int length)
        {
            float[] res = new float[length];

            for(int i = 0; i < length; i++)
            {
                res[i] = *(arr + i);
            }

            return res;
        }

        private unsafe byte[] convert(byte* arr, int length)
        {
            byte[] res = new byte[length];

            for(int i = 0; i < length; i++)
            {
                res[i] = *(arr + i);
            }

            return res;
        }

        private unsafe void updateJoystick(Joystick joystick)
        {
            int numAxes = 0;
            int numButtons = 0;

            float* newAxes = Glfw.GetJoystickAxes((int)joystick, ref numAxes);
            byte* newButtons = Glfw.GetJoystickButtons((int)joystick, ref numButtons);

            joystickAxes[joystick].Clear();
            joystickAxes[joystick].AddRange(numAxes != 0 ? convert(newAxes, numAxes) : new float[1]);

            joystickButtons[joystick].Clear();
            joystickButtons[joystick].AddRange(numButtons != 0 ? convert(newButtons, numButtons) : new byte[1]);
        }

        private void updateJoysticks()
        {
            foreach(KeyValuePair<Joystick, List<float>> de in joystickAxes)
            {
                updateJoystick(de.Key);
            }
        }

        private void updateMouse()
        {
            Vector2 mb = mousePosition;

            Glfw.GetCursorPos(inputSource, ref mouseX, ref mouseY);

            mousePosition.setX((float)(mouseX / halfWidth) - 1);
            mousePosition.setY((float)(mouseY / halfHeight) - 1);

            if (hasBeenUpdated)
            {
                mouseDelta = mousePosition - mb;
            }

            hasBeenUpdated = true;
        }

        public void update()
        {
            updateMouse();
            updateJoysticks();
        }

        public bool getKey(Key code)
        {
            return Glfw.GetKey(inputSource, (int)code) == 1;
        }

        public bool getMouse(Mouse button)
        {
            return Glfw.GetMouseButton(inputSource, (int)button) == 1;
        }

        public double getMouseX()
        {
            return mousePosition.getX();
        }

        public double getMouseY()
        {
            return mousePosition.getY();
        }

        public double getMouseDeltaX()
        {
            return mouseDelta.getX();
        }

        public double getMouseDeltaY()
        {
            return mouseDelta.getY();
        }

        public string getJoystickName(Joystick joystick)
        {
            return Glfw.GetJoystickName((int)joystick);
        }

        public int getNumJoystickAxes(Joystick joystick)
        {
            initJoystick(joystick);
            return joystickAxes[joystick].Count;
        }

        public double getJoystickAxis(Joystick joystick, int axis)
        {
            int numAxes = getNumJoystickAxes(joystick);
            if (axis < 0 || axis >= numAxes)
            {
                return 0.0;
            }
            return (double)joystickAxes[joystick][axis];
        }

        public int getNumJoystickButtons(Joystick joystick)
        {
            initJoystick(joystick);
            return joystickButtons[joystick].Count;
        }

        public bool getJoystickButton(Joystick joystick, int button)
        {
            int numButtons = getNumJoystickButtons(joystick);
            if (button < 0 || button >= numButtons)
            {
                return false;
            }

            Console.WriteLine("Line 157 (OpenGLInput.cs): " + joystickButtons[joystick][button]);            
            return joystickButtons[joystick][button] == 1;
        }

        public Vector2 getMousePos()
        {
            return mousePosition;
        }

        public Vector2 getMouseDelta()
        {
            return mouseDelta;
        }

        public void setMousePos(double x, double y)
        {
            Glfw.SetCursorPos(inputSource, x * halfWidth + halfWidth, y * halfHeight + halfHeight);
        }

        public void setMouse(bool isEnabled)
        {
            Glfw.SetInputMode(inputSource, (int)(State.Cursor), (int)(isEnabled ? State.CursorNormal : State.CursorHidden));
        }
    }
}
