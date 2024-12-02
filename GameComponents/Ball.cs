using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
namespace PingPongGame.GameComponents
{
    public class Ball
    {
        private Texture2D _texture;
        private SoundEffect _hitSound;
        public Vector2 Position;
        private Vector2 _velocity;
        private readonly int _screenWidth, _screenHeight;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)_texture.Width, (int)_texture.Height);

        public Ball(Vector2 position, int screenWidth, int screenHeight)
        {
            Position = position;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _velocity = new Vector2(300, 200); // Initial speed of the ball
        }

        public void LoadContent(ContentManager content, string assetName)
        {
            _texture = content.Load<Texture2D>(assetName);
            _hitSound = content.Load<SoundEffect>("Sounds/beepnegative");
        }

        public void Update(GameTime gameTime, Rectangle playerBounds, Rectangle aiBounds)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            Position += _velocity * (float)deltaTime;

            // Collision with top and bottom of screen
            if (Position.Y <= 0 || Position.Y >= _screenHeight - _texture.Height)
                _velocity.Y *= -1;

            // Collision with paddles
            if (Bounds.Intersects(playerBounds) || Bounds.Intersects(aiBounds))
            {
                _velocity.X *= -1;
                SoundEffectInstance soundInstance = _hitSound.CreateInstance();
                soundInstance.Volume = 0.5f; // Set volume (0.0f to 1.0f)
                soundInstance.Pitch = 0.0f; // Set pitch (-1.0f to 1.0f)
                soundInstance.Pan = 0.0f;   // Set pan (-1.0f for left, 1.0f for right)
                soundInstance.Play();
            }

            // Reset ball position if it goes out of bounds
            if (Position.X <= -1 || Position.X >= _screenWidth + 1)
                ResetPosition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public void ResetPosition()
        {
            Position = new Vector2(_screenWidth / 2, _screenHeight / 2);
            _velocity = new Vector2(300, 200);
        }
    }
}