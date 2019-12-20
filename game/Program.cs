using engine;
using engine.rendering;

namespace Game
{
    public class Game : Scene
    {
        public Game(Window window)
        {

        }

        public override bool update(double delta)
        {
            return false;
        }

        public override void render(double delta)
        {

        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Window window = new Window(1280, 720, "Ghey");
            Engine engine = new Engine(window, new Game(window), 60);
            engine.run();
        }
    }
}
