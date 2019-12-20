namespace engine.input
{
    public class CompoundAxis : IAxis
    {
        private IAxis[] axes;

        public CompoundAxis(IAxis axis1, IAxis axis2) : this(new IAxis[] { axis1, axis2}) { }

        public CompoundAxis(IAxis[] axes)
        {
            this.axes = axes;
        }

        public double getAmount()
        {
            double result = 0.0;
            for (int i = 0; i < axes.Length; i++)
            {
                result += axes[i].getAmount();
            }

            return result;
        }
    }
}
