using System;
using System.Collections.Generic;

using engine.components;
using engine.ecs;
using engine.math;

namespace engine.physics
{
    public class ForceAccumulator : BaseComponent
    {
        public Dictionary<Vector2, Vector2> forceAccum;

        public ForceAccumulator()
        {
            forceAccum = new Dictionary<Vector2, Vector2>();
        }
    }

    public class PhysicalDefinition : BaseComponent
    {
        public double inverseMass;
        public double inverseMomentOfInertia;

        public Vector2 velocity;
        public Vector2 acceleration;

        public double angularVelocity;
        public double angularAcceleration;

        public PhysicalDefinition(double mass)
        {
            double moi = 1; //TODO Calc this

            inverseMass = 1.0 / mass;
            inverseMomentOfInertia = 1.0 / (moi);

            velocity = new Vector2(0, 0);
            acceleration = new Vector2(0, 0);

            angularVelocity = 0;
            angularAcceleration = 0;
        }
    }

    public class RigidBody : Entity
    {
        public RigidBody(Transform transform, double mass)
        {
            addComponent(new TransformComponent(transform));
            addComponent(new PhysicalDefinition(mass));
            addComponent(new ForceAccumulator());
        }

        // this needs to be redone using a different method

        public Vector2 addForce(Vector2 force, Vector2 forceOffset)
        {
            getForceAccum().forceAccum.Add(force, forceOffset);

            return force;
        }

        public void removeForce(Vector2 force)
        {
            getForceAccum().forceAccum.Remove(force);
        }

        //

        private ForceAccumulator getForceAccum()
        {
            return ((ForceAccumulator)this[2]);
        }

        public Transform getTransform()
        {
            return ((TransformComponent)this[0]).transform;
        }

        public double getMass()
        {
            return 1.0 / ((PhysicalDefinition)this[1]).inverseMass;
        }

        public double getMomentOfInertia()
        {
            return 1.0 / ((PhysicalDefinition)this[1]).inverseMomentOfInertia;
        }

        public Vector2 getVelocity()
        {
            return ((PhysicalDefinition)this[1]).velocity;
        }

        public Vector2 getAcceleration()
        {
            return ((PhysicalDefinition)this[1]).acceleration;
        }

        public double getAngluarVelocity()
        {
            return ((PhysicalDefinition)this[1]).angularVelocity;
        }

        public double getAngluarAcceleration()
        {
            return ((PhysicalDefinition)this[1]).angularAcceleration;
        }
    }

    public class RigidBodySimulator : BaseSystem
    {
        public RigidBodySimulator(bool gravity)
        {
            addComponentType(typeof(TransformComponent));
            addComponentType(typeof(PhysicalDefinition));
            addComponentType(typeof(ForceAccumulator));
        }

        public override void componentUpdate(BaseComponent[] components, double delta)
        {
            Transform transform = ((TransformComponent)components[0]).transform;
            PhysicalDefinition physDef = ((PhysicalDefinition)components[1]);
            Dictionary<Vector2, Vector2> forceAccum = ((ForceAccumulator)components[2]).forceAccum;

            MotionIntegrators.forceUpdate(forceAccum, 0 /*TODO: add torque accumulator*/, physDef.inverseMass, physDef.inverseMomentOfInertia, 
                                            ref physDef.acceleration, ref physDef.angularAcceleration);
            
            Vector2 newPosition = transform.getPosition();
            double newAngle = transform.getRotation();

            MotionIntegrators.forestRuth(delta, ref newPosition, ref physDef.velocity, physDef.acceleration);
            MotionIntegrators.rotationForestRuth(delta, ref newAngle, ref physDef.angularVelocity, physDef.angularAcceleration);

            transform.setPosition(newPosition);
            transform.setRotation(newAngle);
        }
    }
}