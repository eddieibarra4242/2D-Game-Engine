using glfw3;

namespace engine.input
{
    public class MouseButton : BaseButton<Mouse>
    {
        private class MouseCommand : Command<Mouse>
        {
            public bool isDown(IInput input, Mouse code)
            {
                return input.getMouse(code);
            }
        }

        public MouseButton(IInput input, Mouse mouseButton) : base(input, new Mouse[] { mouseButton }, new MouseCommand()) { }
        public MouseButton(IInput input, Mouse[] mouseButtons) : base(input, mouseButtons, new MouseCommand()) { }
    }
}
