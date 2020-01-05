using engine.math;

namespace engine.physics
{
    public class Force
    {
        private Vector2 force;

        public Force()
        {
            force = Vector2.zero;   
        }

        public void setForce(Vector2 force)
        {
            this.force = force;
        }

        public Vector2 getForce()
        {
            return force;
        }

        public virtual Vector2 generateForce(RigidBody body)
        {
            return force;
        }
    }
}