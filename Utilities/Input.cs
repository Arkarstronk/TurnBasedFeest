using Microsoft.Xna.Framework.Input;

namespace TurnBasedFeest.Utilities
{
    class Input
    {
        KeyboardState oldKeyState;
        KeyboardState newKeyState;

        public void Update()
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
        }

        public bool Pressed(Keys key)
        {
            if(newKeyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }
    }
}
