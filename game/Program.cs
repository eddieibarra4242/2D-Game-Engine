using engine;
using engine.math;
using engine.rendering;
using engine.ecs;
using engine.components;
using engine.physics;

using OpenTK.Graphics.OpenGL;

using System;
using System.Collections.Generic;

namespace Game
{
    public class VelocityComponent : BaseComponent
    {
        public Vector2 velocity;

        public VelocityComponent(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }

    public class ColliderComponent : BaseComponent
    {
        public AABB collider;

        public ColliderComponent(AABB collider)
        {
            this.collider = collider;
        }
    }

    public class RectSimulator : BaseSystem
    {
        public RectSimulator()
        {
            addComponentType(typeof(TransformComponent));
            addComponentType(typeof(VelocityComponent));
        }
        
        public override void componentUpdate(BaseComponent[] components, double delta)
        {
            Transform transform = ((TransformComponent)components[0]).transform;
            VelocityComponent vel = (VelocityComponent)components[1];

            Vector2 newPosition = transform.getPosition();
            MotionIntegrators.forestRuth(delta, ref newPosition, ref vel.velocity, new Vector2(0, 0));
            transform.setPosition(newPosition);
        }
    }

    public class RectColliderSys : BaseSystem
    {
        private List<AABB> colliders;

        public RectColliderSys()
        {
            addComponentType(typeof(TransformComponent));
            addComponentType(typeof(ColliderComponent));

            colliders = new List<AABB>();
        }

        public override void componentUpdate(BaseComponent[] components, double delta)
        {
            Transform transform = ((TransformComponent)components[0]).transform;
            ColliderComponent coll = (ColliderComponent)components[1];

            coll.collider.translate(transform.getPosition());

            colliders.Add(coll.collider);
        }

        public override void postComponentUpdate(double delta)
        {
            for(int i = 0; i < colliders.Count; i++)
            {
                for(int j = i + 1; j < colliders.Count; j++)
                {
                    Console.WriteLine(colliders[i].intersect(colliders[j]));
                }
            }

            colliders.Clear();
        }
    }

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
        public Game(Window window)
        {
            getECS().makeEntity(new TransformComponent(new Transform(new Vector2(-1, -1), 0, new Vector2(0.2, 0.2))),
            new VelocityComponent(new Vector2(0.3, 0.3)),
            new ColliderComponent(new AABB(new Vector2(-0.2, -0.2), new Vector2(0.2, 0.2))));
            getECS().makeEntity(new TransformComponent(new Transform(new Vector2(1, 1), 0, new Vector2(0.2, 0.2))),
            new VelocityComponent(new Vector2(-0.3, -0.3)),
            new ColliderComponent(new AABB(new Vector2(-0.2, -0.2), new Vector2(0.2, 0.2))));

            addSimulatorSystem(new RectSimulator());
            addCollisionDetectionSystem(new RectColliderSys());
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
