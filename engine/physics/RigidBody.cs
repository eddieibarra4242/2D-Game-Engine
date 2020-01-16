using System;
using System.Collections.Generic;

using engine.components;
using engine.ecs;
using engine.math;

namespace engine.physics
{
    public class ForceAndTorqueAccumulator : BaseComponent
    {
        public Dictionary<Force, Vector2> forceAccum;
        public List<Torque> torques;

        public ForceAndTorqueAccumulator()
        {
            forceAccum = new Dictionary<Force, Vector2>();
            torques = new List<Torque>();
        }
    }

    public class PhysicalDefinition : BaseComponent
    {
        public RigidBody parent;

        public double inverseMass;
        public double inverseMomentOfInertia;

        public Vector2 velocity;
        public Vector2 acceleration;

        public double angularVelocity;
        public double angularAcceleration;

        public PhysicalDefinition(RigidBody parent, double mass)
        {
            this.parent = parent;

            double moi = mass * Math.Pow(parent.getTransform().getScale().getX() * 2, 2) / 12; //TODO Calc this

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
            addComponent(new PhysicalDefinition(this, mass));
            addComponent(new ForceAndTorqueAccumulator());
        }

        public Force addForce(Force force, Vector2 forceOffset)
        {
            getForceAccum().forceAccum.Add(force, forceOffset);
            return force;
        }

        public void removeForce(Force force)
        {
            getForceAccum().forceAccum.Remove(force);
        }

        public Torque addTorque(Torque torque)
        {
            getForceAccum().torques.Add(torque);
            return torque;
        }

        public void removeTorque(Torque torque)
        {
            getForceAccum().torques.Remove(torque);
        }

        private ForceAndTorqueAccumulator getForceAccum()
        {
            return ((ForceAndTorqueAccumulator)this[2]);
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
        private List<Vector2> forces;
        private List<Vector2> offsets;

        public RigidBodySimulator()
        {
            addComponentType(typeof(TransformComponent));
            addComponentType(typeof(PhysicalDefinition));
            addComponentType(typeof(ForceAndTorqueAccumulator));

            forces = new List<Vector2>();
            offsets = new List<Vector2>();
        }

        public override void componentUpdate(BaseComponent[] components, double delta)
        {
            Transform transform = ((TransformComponent)components[0]).transform;
            PhysicalDefinition physDef = ((PhysicalDefinition)components[1]);
            ForceAndTorqueAccumulator forceTorqueAccum = ((ForceAndTorqueAccumulator)components[2]);

            foreach(KeyValuePair<Force, Vector2> kvp in forceTorqueAccum.forceAccum)
            {
                forces.Add(kvp.Key.generateForce(physDef.parent, kvp.Value));
                offsets.Add(kvp.Value);
            }

            double torqueAccum = 0;

            foreach(Torque torque in forceTorqueAccum.torques)
            {
                torqueAccum += torque.generateTorque(physDef.parent);
            }

            MotionIntegrators.forceUpdate(forces, offsets, torqueAccum, physDef.inverseMass, physDef.inverseMomentOfInertia, 
                                            ref physDef.acceleration, ref physDef.angularAcceleration);

            forces.Clear();
            offsets.Clear();
            
            Vector2 newPosition = transform.getPosition();
            double newAngle = transform.getRotation();

            MotionIntegrators.forestRuth(delta, ref newPosition, ref physDef.velocity, physDef.acceleration);
            MotionIntegrators.rotationForestRuth(delta, ref newAngle, ref physDef.angularVelocity, physDef.angularAcceleration);

            transform.setPosition(newPosition);
            transform.setRotation(newAngle);
        }
    }
}