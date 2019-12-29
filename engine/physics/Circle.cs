using engine.math;

namespace engine.physics
{
    public class Circle
    {
        private Vector2 center;
        private double radius;

        public Circle(Vector2 center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public void translate(Vector2 position)
        {
            this.center = position;
        }

        public Vector2 intersect(Circle circle)
        {
            Vector2 centerDist = center - circle.getCenter();
            double idealDist = (radius + circle.getRadius());

            if(centerDist.length() <= idealDist)
            {
                return centerDist.normalized() * (idealDist - centerDist.length());
            }

            return new Vector2(0, 0);
        }

        public Vector2 getCenter()
        {
            return center;
        }

        public double getRadius()
        {
            return radius;
        }

        public void setCenter(Vector2 center)
        {
            this.center = center;
        }

        public void setRadius(double radius)
        {
            this.radius = radius;
        }
    }
}