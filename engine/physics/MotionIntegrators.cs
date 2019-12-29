using engine.math;

using System;

namespace engine.physics
{
    public static class MotionIntegrators
    {
        public static void forceUpdate(Vector2 force, double mass, ref Vector2 acceleration) 
        {
            acceleration = force / mass;
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
            double frCoefficient = 1.0/(2.0 - Math.Pow(2.0, 1.0/3.0));
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
    }
}