using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.input
{
    public interface IButton
    {
        /**
	     * Returns whether this button is currently pressed
	     * 
	     * @return True if the button is pressed, false otherwise.
	     */
        bool isDown();
    }
}
