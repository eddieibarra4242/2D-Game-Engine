namespace engine.math
{
    public class Transform
    {
        public Vector2 position;
        public float rotation;
        public Vector2 scale;

        public Transform()
        {
            this.position = new Vector2(0, 0);
            this.rotation = 0;
            this.scale = new Vector2(1, 1);
        }

        public Matrix3 getTramsformationMatrix()
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

        public float getRotation()
        {
            return rotation;
        }

        public void setRotation(float rotation)
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