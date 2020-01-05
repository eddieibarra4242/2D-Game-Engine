namespace engine.physics
{
    public class Torque
    {
        private double torque;

        public Torque()
        {
            torque = 0;   
        }

        public void setTorque(double torque)
        {
            this.torque = torque;
        }

        public double getTorque()
        {
            return torque;
        }

        public virtual double generateTorque(RigidBody body)
        {
            return torque;
        }
    }
}