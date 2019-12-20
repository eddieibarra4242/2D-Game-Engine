using System;
using engine.rendering;

namespace engine
{
    public class Engine
    {
        private Window window;
        private Scene game;
        private int fpslock;

        public static int fps;

        public Engine(Window window, Scene game, int fpslock)
        {
            this.window = window;
            this.game = game;
            this.fpslock = fpslock;
            fps = fpslock;
        }

        public void run()
        {
            bool isRunning = true;
            bool render = false;
            double lastTime = Environment.TickCount / 1000.0;
            double lastRender = lastTime;

            double unprocessedTime = 0.0;
            double frameTime = fpslock == 0 ? 0 : (1.0 / (double)fpslock);

            int frames = 0;
            double timer = 0.0;

            while(isRunning)
            {
                double startTime = Environment.TickCount / 1000.0;
                double passedTime = startTime - lastTime;
                lastTime = startTime;

                unprocessedTime += passedTime;
                timer += passedTime;

                while(unprocessedTime > frameTime)
                {
                    unprocessedTime -= passedTime;
                    render = true;

                    if(window.shouldClose() || game.engineUpdate(passedTime))
                    {
                        isRunning = false;
                    }

                    window.update();
                }

                if(render)
                {
                    double startRender = Environment.TickCount / 1000.0;
                    double deltaRender = startRender - lastRender;
                    lastRender = startRender;

                    game.engineRender(deltaRender);
                    window.present();
                    frames++;
                    render = false;
                }

                if(timer >= 1.0)
                {
                    fps = frames;
                    frames = 0;
                    timer = 0;
                }
            }

            window.destroy();
        }
    }
}