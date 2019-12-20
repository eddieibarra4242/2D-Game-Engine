namespace engine
{
    public class Util
    {
        public static double clamp(double val, double min, double max)
        {
            if (val > max)
            {
                return max;
            }

            if(val < min)
            {
                return min;
            }

            return val;
        }
    }
}