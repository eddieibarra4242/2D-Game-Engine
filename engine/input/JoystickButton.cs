using glfw3;

namespace engine.input
{
    public class JoystickButton : BaseButton<int>
    {
        private class JoyCommand : Command<int>
        {
            private Joystick joystick;

            public JoyCommand(Joystick joystick)
            {
                this.joystick = joystick;
            }

            public bool isDown(IInput input, int code)
            {
                return input.getJoystickButton(joystick, code);
            }
        }

        public JoystickButton(IInput input, Joystick joystick, int joyButton) : base(input, new int[] { joyButton }, new JoyCommand(joystick)) { }
        public JoystickButton(IInput input, Joystick joystick, int[] joyButtons) : base(input, joyButtons, new JoyCommand(joystick)) { }
    }
}
