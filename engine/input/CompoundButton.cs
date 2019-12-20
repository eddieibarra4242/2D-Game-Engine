namespace engine.input
{
    public class CompoundButton : IButton
    {
        private IButton[] buttons;

        public CompoundButton(IButton button1, IButton button2) : this(new IButton[] { button1, button2 }) { }

        public CompoundButton(IButton[] buttons)
        {
            this.buttons = buttons;
        }

        public bool isDown()
        {
            for(int i = 0; i < buttons.Length; i++)
            {
                if(buttons[i].isDown())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
