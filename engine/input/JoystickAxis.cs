using glfw3;

namespace engine.input
{
    public class JoystickAxis : IAxis
    {
        private IInput input;
        private Joystick joystick;
        private int[] joystickAxes;

        public JoystickAxis(IInput input, Joystick joystick, int joystickAxis) : this(input, joystick, new int[] { joystickAxis }) { }

        public JoystickAxis(IInput input, Joystick joystick, int[] joystickAxes)
        {
            this.input = input;
            this.joystick = joystick;
            this.joystickAxes = joystickAxes;
        }

        public double getAmount()
        {
            if(joystickAxes == null)
            {
                return 0.0;
            }

            double result = 0.0;
            for(int i = 0; i < joystickAxes.Length; i++)
            {
                result += input.getJoystickAxis(joystick, joystickAxes[i]);
            }

            return Util.clamp(result, -1.0, 1.0);
        }
    }
}
