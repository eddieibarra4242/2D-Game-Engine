using engine;
using engine.math;
using engine.rendering;
using engine.ecs;
using engine.components;
using engine.input;

using OpenTK.Graphics.OpenGL;

namespace Game
{
    public class InputComponent : BaseComponent
    {
        public KeyButton up;
        public KeyButton down;
        public KeyButton left;
        public KeyButton right;
    }

    public class MovementSystem : BaseSystem
    {
        public MovementSystem() : base()
        {
            addComponentType(typeof(TransformComponent));
            addComponentType(typeof(InputComponent));
        }

        private void move(Transform transform, Vector2 dir, double amt)
        {
            transform.setPosition(transform.getPosition() + (dir * amt));
        }

        public override void componentUpdate(BaseComponent[] components, double delta)
        {
            Transform transform = ((TransformComponent)components[0]).transform;
            InputComponent inputer = (InputComponent)components[1];

            if(inputer.up.isDown())
            {
                move(transform, new Vector2(0, 1), 0.05f);
            }

            if(inputer.down.isDown())
            {
                move(transform, new Vector2(0, -1), 0.05f);
            }

            if(inputer.left.isDown())
            {
                move(transform, new Vector2(-1, 0), 0.05f);
            }

            if(inputer.right.isDown())
            {
                move(transform, new Vector2(1, 0), 0.05f);
            }
        }
    }

    public class RectRenderer : BaseSystem
    {
        private Vector2[] vertices;

        public RectRenderer() : base()
        {
            addComponentType(typeof(TransformComponent));
            vertices = new Vector2[] {new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1)};
        }

        public override void preComponentUpdate(double delta)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public override void componentUpdate(BaseComponent[] components, double delta)
        {
            Transform transform = ((TransformComponent)components[0]).transform;
            Matrix3 transformMat = transform.getTransformationMatrix();
            Vector2[] transformedVertices = new Vector2[vertices.Length];

            for(int i = 0; i < vertices.Length; i++)
            {
                transformedVertices[i] = transformMat.mul(vertices[i]);
            }

            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(transformedVertices[0].getX() / Window.getAR(), transformedVertices[0].getY());
            GL.Vertex2(transformedVertices[1].getX() / Window.getAR(), transformedVertices[1].getY());
            GL.Vertex2(transformedVertices[2].getX() / Window.getAR(), transformedVertices[2].getY());
            GL.Vertex2(transformedVertices[3].getX() / Window.getAR(), transformedVertices[3].getY());

            GL.End();
        }
    }

    public class Game : Scene
    {
        public Game(Window window)
        {
            TransformComponent transformComponent = new TransformComponent(new Transform());
            transformComponent.transform.setScale(new Vector2(0.3f, 0.3f));

            InputComponent inputComp = new InputComponent();
            inputComp.up = new KeyButton(window.getInput(), glfw3.Key.W);
            inputComp.down = new KeyButton(window.getInput(), glfw3.Key.S);
            inputComp.left = new KeyButton(window.getInput(), glfw3.Key.A);
            inputComp.right = new KeyButton(window.getInput(), glfw3.Key.D);

            getECS().makeEntity(transformComponent, inputComp);

            addSceneSystem(new MovementSystem());

            getRenderPipeLine().addSystem(new RectRenderer());
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
            Window window = new Window(1280, 720, "2D Engine");
            Engine engine = new Engine(window, new Game(window), 60);
            engine.run();
        }
    }
}
