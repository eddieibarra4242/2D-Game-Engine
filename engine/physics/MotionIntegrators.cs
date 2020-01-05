using engine.math;
using System;
using System.Collections.Generic;

namespace engine.physics
{
    public static class MotionIntegrators
    {
        public static void forceUpdate(List<Vector2> forces, List<Vector2> forceOffsets, double torqueAccum, double inverseMass, double inverseMomentOfInertia, ref Vector2 acceleration, ref double angularAcceleration) 
        {
            Vector2 forceAccum = new Vector2(0, 0);

            for(int i = 0; i < forces.Count; i++)
            {
                Vector2 force = forces[i];
                Vector2 forceOffset = forceOffsets[i];

                if(force == null || forceOffset == null)
                {
                    continue; // I'm not gonna babysit problem children
                }

                if(forceOffset == Vector2.zero)
                {
                    forceAccum += force;
                    continue;
                }

                torqueAccum += forceOffset.cross(force);
                forceAccum += forceOffset.normalized() * (force.dot(forceOffset));
            }

            acceleration = forceAccum * inverseMass;
            angularAcceleration = torqueAccum * inverseMomentOfInertia;
        }

        public static void verlet(double delta, ref Vector2 position, ref Vector2 velocity, Vector2 acceleration)
        {
            double halfDelta = delta * 0.5;

            position += velocity * halfDelta;
            velocity += acceleration * delta;
            position += velocity * halfDelta;
        }

        public static void forestRuth(double delta, ref Vector2 position, ref Vector2 velocity, Vector2 acceleration)
        {
            double frCoefficient = 1.0 / (2.0 - Math.Pow(2.0, 1.0/3.0));
            double frComplement = 1.0 - 2.0*frCoefficient;
            verlet(delta*frCoefficient, ref position, ref velocity, acceleration);
            verlet(delta*frComplement, ref position, ref velocity, acceleration);
            verlet(delta*frCoefficient, ref position, ref velocity, acceleration);
        }

        public static void euler(double delta, ref Vector2 position, ref Vector2 velocity, Vector2 acceleration)
        {
            velocity += acceleration * delta;
            position += (velocity * delta) + (acceleration * delta * delta);
        }

        public static void rotationVerlet(double delta, ref double angle, ref double angularVelocity, double angularAcceleration)
        {
            double halfDelta = delta * 0.5;

            angle += angularVelocity * halfDelta;
            angularVelocity += angularAcceleration * delta;
            angle += angularVelocity * halfDelta;
        }

        public static void rotationForestRuth(double delta, ref double angle, ref double angularVelocity, double angularAcceleration)
        {
            double frCoefficient = 1.0/(2.0 - Math.Pow(2.0, 1.0/3.0));
            double frComplement = 1.0 - 2.0*frCoefficient;
            rotationVerlet(delta*frCoefficient, ref angle, ref angularVelocity, angularAcceleration);
            rotationVerlet(delta*frComplement, ref angle, ref angularVelocity, angularAcceleration);
            rotationVerlet(delta*frCoefficient, ref angle, ref angularVelocity, angularAcceleration);
        }

        public static void rotationEuler(double delta, ref double angle, ref double angularVelocity, double angularAcceleration)
        {
            angularVelocity += angularAcceleration * delta;
            angle += (angularVelocity * delta) + (angularAcceleration * delta * delta);
        }
    }
}