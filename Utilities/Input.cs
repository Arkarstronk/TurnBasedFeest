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

        public bool Released(Keys key)
        {
            if(oldKeyState.IsKeyDown(key) && newKeyState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }
    }
}
