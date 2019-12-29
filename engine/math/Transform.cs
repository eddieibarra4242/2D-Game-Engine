namespace engine.math
{
    public class Transform
    {
        private Vector2 position;
        private double rotation;
        private Vector2 scale;

        public Transform() : this(new Vector2(0, 0), 0, new Vector2(1, 1)) {}

        public Transform(Vector2 postion, float angle, Vector2 scale)
        {
            this.position = postion;
            this.rotation = angle;
            this.scale = scale;
        }

        public Matrix3 getTransformationMatrix()
        {
            Matrix3 pos = new Matrix3().initTranslation(position);
            Matrix3 rot = new Matrix3().initRotation(rotation);
            Matrix3 scale = new Matrix3().initScale(this.scale);

            return pos.mul(rot.mul(scale));
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public double getRotation()
        {
            return rotation;
        }

        public void setRotation(double rotation)
        {
            this.rotation = rotation;
        }

        public Vector2 getScale()
        {
            return scale;
        }

        public void setScale(Vector2 scale)
        {
            this.scale = scale;
        }
    }
}