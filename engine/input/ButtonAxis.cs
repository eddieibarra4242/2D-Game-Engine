namespace engine.input
{
    public class ButtonAxis : IAxis
    {
        private IButton positive;
        private IButton negative;

        public ButtonAxis(IButton negative, IButton positive)
        {
            this.negative = negative;
            this.positive = positive;
        }

        public double getAmount()
        {
            double result = 0.0;

            if (positive.isDown())
            {
                result += 1.0;
            }

            if (negative.isDown())
            {
                result -= 1.0;
            }

            return result;
        }
    }
}
