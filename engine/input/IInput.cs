using engine.math;

using glfw3;

namespace engine.input
{
    public interface IInput
    {
        /** Updates the input system */
        void update();

        /**
         * Gets whether or not a particular key is currently pressed.
         * 
         * @param code
         *            The key to test
         * @return Whether or not key is currently pressed.
         */
        bool getKey(Key code);

        /**
         * Gets whether or not a particular mouse button is currently pressed.
         * 
         * @param button
         *            The button to test
         * @return Whether or not the button is currently pressed.
         */
        bool getMouse(Mouse button);

        /**
         * Gets the location of the mouse cursor on x, in pixels.
         * 
         * @return The location of the mouse cursor on x, in pixels
         */
        double getMouseX();

        /**
         * Gets the location of the mouse cursor on y, in pixels.
         * 
         * @return The location of the mouse cursor on y, in pixels
         */
        double getMouseY();

        Vector2 getMousePos();

        /**
         * Gets the amount the mouse has moved since the previous update on X.
         * 
         * @return The amount the mouse has moved since the previous update on X.
         */
        double getMouseDeltaX();

        /**
         * Gets the amount the mouse has moved since the previous update on Y.
         * 
         * @return The amount the mouse has moved since the previous update on Y.
         */
        double getMouseDeltaY();

        Vector2 getMouseDelta();

        /**
         * Gets a string describing a particular joystick.
         * 
         * @param joystick
         *            The joystick of interest.
         * @return A string describing the specified joystick.
         */
        string getJoystickName(Joystick joystick);

        /**
         * Gets the number of axes a joystick offers.
         * 
         * @param joystick
         *            The joystick of interest.
         * @return The number of axes the joystick offers.
         */
        int getNumJoystickAxes(Joystick joystick);

        /**
         * Gets the value of a joystick's axis.
         * 
         * @param joystick
         *            The joystick of interest.
         * @param axis
         *            The axis of interest.
         * @return The value of the joystick's axis.
         */
        double getJoystickAxis(Joystick joystick, int axis);

        /**
         * Gets the number of buttons a joystick offers.
         * 
         * @param joystick
         *            The joystick of interest.
         * @return The number of buttons the joystick offers.
         */
        int getNumJoystickButtons(Joystick joystick);

        /**
         * Gets the whether a button on a joystick is pressed.
         * 
         * @param joystick
         *            The joystick of interest.
         * @param button
         *            The button of interest.
         * @return Whether a button on a joystick is pressed.
         */
        bool getJoystickButton(Joystick joystick, int button);

        void setMousePos(double x, double y);

        void setMouse(bool isEnabled);
    }
}
