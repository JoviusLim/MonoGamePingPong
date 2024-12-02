using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using PingPongGame.GameComponents;
using PingPongGame.Managers;

namespace PingPongGame.States
{
    public class PlayState
    {
        private Paddle _playerPaddle, _aiPaddle, _player2Paddle;
        private Ball _ball;
        private int _playerScore, _aiScore;
        private bool _isTwoPlayer;
        private SpriteFont _font;
        private SoundEffect _scoreSound;
        private GameStateManager _gameStateManager;
        private int _screenWidth, _screenHeight;

        public bool IsPlaying { get; private set; }

        public PlayState(GameStateManager gameStateManager, bool isTwoPlayer, int screenWidth, int screenHeight)
        {
            _gameStateManager = gameStateManager;
            _isTwoPlayer = isTwoPlayer;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _playerScore = 0;
            _aiScore = 0;

            _playerPaddle = new Paddle(new Vector2(5, _screenHeight - 100), true);
            _aiPaddle = new Paddle(new Vector2(_screenWidth - 20, _screenHeight - 100), false);
            _player2Paddle = new Paddle(new Vector2(_screenWidth - 20, _screenHeight - 100), true);
            _ball = new Ball(new Vector2(_screenWidth / 2, _screenHeight / 2), _screenWidth, _screenHeight);
        }

        public void LoadContent(ContentManager content)
        {
            var paddleTexture = "Sprites/Paddle";
            var ballTexture = "Sprites/Ball";

            _playerPaddle.LoadContent(content, paddleTexture);
            _aiPaddle.LoadContent(content, paddleTexture);
            _player2Paddle.LoadContent(content, paddleTexture);
            _ball.LoadContent(content, ballTexture);

            _font = content.Load<SpriteFont>("font");
            _scoreSound = content.Load<SoundEffect>("Sounds/beepnegative");
        }

        public void Update(GameTime gameTime,InputManager inputManager)
        {
            _playerPaddle.Update(gameTime, inputManager, _screenHeight, false);

            if (_isTwoPlayer)
            {
                _player2Paddle.Update(gameTime, inputManager, _screenHeight, true);
            }
            else
            {
                _aiPaddle.UpdateAI(gameTime, _ball.Position, _screenHeight);
            }

            _ball.Update(gameTime, _playerPaddle.Bounds, _isTwoPlayer ? _player2Paddle.Bounds : _aiPaddle.Bounds);

            if (_ball.Position.X <= 0)
            {
                _aiScore++;
                SoundEffectInstance soundInstance = _scoreSound.CreateInstance();
                soundInstance.Volume = 0.5f; // Set volume (0.0f to 1.0f)
                soundInstance.Pitch = -1.0f; // Set pitch (-1.0f to 1.0f)
                soundInstance.Pan = 0.0f;   // Set pan (-1.0f for left, 1.0f for right)
                soundInstance.Play();
            }
            if (_ball.Position.X >= _screenWidth)
            {
                _playerScore++;
                SoundEffectInstance soundInstance = _scoreSound.CreateInstance();
                soundInstance.Volume = 0.5f; // Set volume (0.0f to 1.0f)
                soundInstance.Pitch = 1.0f; // Set pitch (-1.0f to 1.0f)
                soundInstance.Pan = 0.0f;   // Set pan (-1.0f for left, 1.0f for right)
                soundInstance.Play();
            }

            if (inputManager.IsKeyPressed(Keys.Escape))
                _gameStateManager.ChangeState(GameState.Menu);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _playerPaddle.Draw(spriteBatch);
            if (_isTwoPlayer)
                _player2Paddle.Draw(spriteBatch);
            else
                _aiPaddle.Draw(spriteBatch);
            _ball.Draw(spriteBatch);

            if (!_isTwoPlayer)
            {
                spriteBatch.DrawString(_font, $"Player: {_playerScore}", new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(_font, $"AI: {_aiScore}", new Vector2(_screenWidth - 100, 10), Color.White);
            }
            else
            {
                spriteBatch.DrawString(_font, $"Player 1: {_playerScore}", new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(_font, $"Player 2: {_aiScore}", new Vector2(_screenWidth - 100, 10), Color.White);
            }
        }

        public void StartGame(bool isTwoPlayer)
        {
            IsPlaying = true;
            _isTwoPlayer = isTwoPlayer;

            _playerPaddle.ResetPosition(_screenHeight);
            _aiPaddle.ResetPosition(_screenHeight);
            _player2Paddle.ResetPosition(_screenHeight);
            _ball.ResetPosition();

            _playerScore = 0;
            _aiScore = 0;
        }

        public void StopGame()
        {
            IsPlaying = false;
        }
    }
}
