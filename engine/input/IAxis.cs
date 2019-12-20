﻿namespace engine.input
{
    public interface IAxis
    {
        /**
	     * Gets the current value along the axis. -1 is smallest input, 1 is largest
	     * input.
	     * 
	     * @return The current value of the axis in the range (-1, 1).
	     */
        double getAmount();
    }
}
