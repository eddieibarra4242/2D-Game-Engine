using glfw3;

namespace engine.input
{
    public class KeyButton : BaseButton<Key>
    {
        private class KeyCommand : Command<Key>
        {
            public bool isDown(IInput input, Key code)
            {
                return input.getKey(code);
            }
        }

        public KeyButton(IInput input, Key keyButton) : base(input, new Key[] { keyButton }, new KeyCommand()) { }
        public KeyButton(IInput input, Key[] keyButtons) : base(input, keyButtons, new KeyCommand()) { }
    }
}
