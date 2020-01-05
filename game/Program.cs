using engine;
using engine.math;
using engine.rendering;
using engine.ecs;
using engine.components;
using engine.physics;

using OpenTK.Graphics.OpenGL;

namespace Game
{
    public class RectRenderer : BaseSystem
    {
        private Vector2[] vertices;

        public RectRenderer() : base()
        {
            addComponentType(typeof(TransformComponent));
            vertices = new Vector2[] {new Vector2(-1, -1),
            new Vector2( 1, -1),
            new Vector2( 1,  1),
            new Vector2(-1,  1)};
        }

        public override void preComponentUpdate(double delta)
        {
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

            GL.PushMatrix();
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(transformedVertices[0].getX() / Window.getAR(), transformedVertices[0].getY());
            GL.TexCoord2(1, 1);
            GL.Vertex2(transformedVertices[1].getX() / Window.getAR(), transformedVertices[1].getY());
            GL.TexCoord2(1, 0);
            GL.Vertex2(transformedVertices[2].getX() / Window.getAR(), transformedVertices[2].getY());
            GL.TexCoord2(0, 0);
            GL.Vertex2(transformedVertices[3].getX() / Window.getAR(), transformedVertices[3].getY());
            GL.TexCoord2(0, 1);

            GL.End();
            GL.PopMatrix();
        }
    }

    public class Gravity : Force
    {
        private Vector2 g;

        public Gravity() : base()
        {
            this.g = new Vector2(0, -9.81);
        }

        public override Vector2 generateForce(RigidBody body)
        {
            return g * body.getMass();
        }
    }

    public class Game : Scene
    {
        public Game(Window window)
        {
            Gravity g = new Gravity();

            RigidBody r1 = new RigidBody(new Transform(new Vector2(-0.75, 0), 0, new Vector2(0.2, 0.2)), 1);
            RigidBody r2 = new RigidBody(new Transform(new Vector2(0.75, 0), 0, new Vector2(0.2, 0.2)), 1);

            r1.addForce(g, new Vector2(0, 0));
            r2.addForce(g, new Vector2(0, 0));

            getECS().addEntity(r1);
            getECS().addEntity(r2);

            addSimulatorSystem(new RigidBodySimulator(true));

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
