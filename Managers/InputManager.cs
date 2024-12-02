using Microsoft.Xna.Framework.Input;

namespace PingPongGame.Managers
{
    public class InputManager
    {
        private KeyboardState _currentKeyState, _previousKeyState;

        public void Update()
        {
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
        }

        // Check if a key is currently pressed
        public bool IsKeyDown(Keys key) => _currentKeyState.IsKeyDown(key);

        // Check if a key was just pressed
        public bool IsKeyPressed(Keys key) => _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
    }
}