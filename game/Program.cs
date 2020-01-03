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

    public class Game : Scene
    {
        RigidBody r1;
        RigidBody r2;
        Vector2 r1Force;
        Vector2 r2Force;

        public Game(Window window)
        {
            Vector2 g = new Vector2(0, -9.81);

            r1 = new RigidBody(new Transform(new Vector2(-0.75, 0), 0, new Vector2(0.2, 0.2)), 1);
            r2 = new RigidBody(new Transform(new Vector2(0.75, 0), 0, new Vector2(0.2, 0.2)), 1);

            r1Force = r1.addForce(g * r1.getMass(), new Vector2(0, 0));
            r2Force = r2.addForce(g * r2.getMass(), new Vector2(0, 0));

            getECS().addEntity(r1);
            getECS().addEntity(r2);

            addSimulatorSystem(new RigidBodySimulator(true));

            getRenderPipeLine().addSystem(new RectRenderer());
        }

        private bool r1l = true;
        private bool r2l = true;

        private double bounds = 0.2;

        public override bool update(double delta)
        {
            if((r1.getTransform().getPosition().getY() <= -bounds || r1.getTransform().getPosition().getY() >= bounds) && r1l)
            {
                r1.removeForce(r1Force);
                r1Force *= -1;
                r1Force = r1.addForce(r1Force, Vector2.zero);
                r1l = false;
            }

            if((r2.getTransform().getPosition().getY() <= -bounds || r2.getTransform().getPosition().getY() >= bounds) && r2l)
            {
                r2.removeForce(r2Force);
                r2Force *= -1;
                r2Force = r2.addForce(r2Force, Vector2.zero);
                r2l = false;
            }

            if(r1.getTransform().getPosition().getY() > -bounds && r1.getTransform().getPosition().getY() < bounds)
            {
                r1l = true;
            }

            if(r2.getTransform().getPosition().getY() > -bounds && r2.getTransform().getPosition().getY() < bounds)
            {
                r2l = true;
            }

            return r1.getVelocity().length() > 20 || r2.getVelocity().length() > 20;
        }

        public override void render(double delta)
        {

        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Window window = new Window(1600, 900, "2D Engine");
            Engine engine = new Engine(window, new Game(window), 60);
            engine.run();
        }
    }
}
