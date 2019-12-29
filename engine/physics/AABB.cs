using engine.math;

namespace engine.physics
{
    public class AABB
    {
        private Vector2 min;
        private Vector2 max;

        public AABB(Vector2 min, Vector2 max)
        {
            this.min = Vector2.min(min, max);
            this.max = Vector2.max(min, max);
        }

        public void translate(Vector2 position)
        {
            Vector2 half = (max - min) * 0.5;

            this.min = position - half;
            this.max = position + half;
        }

        public Vector2 intersect(AABB other) // when things align this doesn't work
        {
            Vector2 checkMins = Vector2.max(min, other.getMin());
            Vector2 checkMaxs = Vector2.min(max, other.getMax());

            if(min.getX() == other.getMin().getX() || max.getX() == other.getMax().getX())
            {
                checkMins.setX(0);
                checkMaxs.setX(0);
            }

            if(min.getY() == other.getMin().getY() || max.getY() == other.getMax().getY())
            {
                checkMins.setY(0);
                checkMaxs.setY(0);
            }

            return Vector2.max(checkMaxs - checkMins, new Vector2(0, 0));
        }

        public Vector2 getMin()
        {
            return min;
        }

        public Vector2 getMax()
        {
            return max;
        }
    }
}