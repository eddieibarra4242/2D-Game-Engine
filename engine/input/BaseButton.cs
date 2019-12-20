namespace engine.input
{
    public abstract class BaseButton<T> : IButton
    {
        public interface Command<CodeType>
        {
            bool isDown(IInput input, CodeType code);
        }

        private IInput input;
        private T[] codes;
        private Command<T> command;

        public BaseButton(IInput input, T[] codes, Command<T> command)
        {
            this.input = input;
            this.codes = codes;
            this.command = command;
        }

        public bool isDown()
        {
            if(codes == null)
            {
                return false;
            }

            for(int i = 0; i < codes.Length; i++)
            {
                if(command.isDown(input, codes[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
