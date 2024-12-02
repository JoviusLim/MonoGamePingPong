using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PingPongGame.Managers;

namespace PingPongGame.GameComponents
{
    public class Paddle
    {
        private Texture2D _texture;
        private Vector2 _position;
        private bool _isPlayerControlled;

        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, (int)_texture.Width, (int)_texture.Height);

        public Paddle(Vector2 position, bool isPlayerControlled)
        {
            _position = position;
            _isPlayerControlled = isPlayerControlled;
        }

        public void LoadContent(ContentManager content, string assetName)
        {
            _texture = content.Load<Texture2D>(assetName);
        }

        public void Update(GameTime gameTime, InputManager inputManager, int screenHeight, bool isPlayer2)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (!_isPlayerControlled) return;

            Keys upKey = isPlayer2 ? Keys.Up : Keys.W;
            Keys downKey = isPlayer2 ? Keys.Down : Keys.S;

            if (inputManager.IsKeyDown(upKey))
                _position.Y -= 300 * (float)deltaTime;
            if (inputManager.IsKeyDown(downKey))
                _position.Y += 300 * (float)deltaTime;

            // Clamp the paddle with the screen height
            _position.Y = MathHelper.Clamp(_position.Y, 0, screenHeight - _texture.Height);
        }

        public void UpdateAI(GameTime gameTime, Vector2 ballPosition, int screenHeight)
        {
            if (_isPlayerControlled) return;

            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (_position.Y + _texture.Height / 2 < ballPosition.Y)
                _position.Y += 200 * (float)deltaTime;
            if (_position.Y + _texture.Height / 2 > ballPosition.Y)
                _position.Y -= 200 * (float)deltaTime;

            // Clamp the paddle with the screen height
            _position.Y = MathHelper.Clamp(_position.Y, 0, screenHeight - _texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public void ResetPosition(int screenHeight)
        {
            _position = new Vector2(_position.X, (screenHeight - _texture.Height) / 2);
        }
    }
}