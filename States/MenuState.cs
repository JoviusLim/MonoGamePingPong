using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PingPongGame.Managers;

namespace PingPongGame.States
{
    public class MenuState
    {
        private SpriteFont _font;
        private GameStateManager _gameStateManager;

        public MenuState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("font");
        }

        public void Update(InputManager inputManager)
        {
            if (inputManager.IsKeyPressed(Keys.D1))
            {
                _gameStateManager.ChangeState(GameState.Play);
                _gameStateManager.ChangeMode(GameMode.OnePlayer);
            }
            if (inputManager.IsKeyPressed(Keys.D2))
            {
                _gameStateManager.ChangeState(GameState.Play);
                _gameStateManager.ChangeMode(GameMode.TwoPlayer);
            }
            if (inputManager.IsKeyPressed(Keys.Escape))
                _gameStateManager.ChangeState(GameState.Exit);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string title = "Ping Pong Game";
            string option1 = "1. Player vs AI";
            string option2 = "2. Player vs Player";
            string exit = "Press Escape to Exit";

            spriteBatch.DrawString(_font, title, new Vector2(300, 100), Color.White);
            spriteBatch.DrawString(_font, option1, new Vector2(300, 200), Color.White);
            spriteBatch.DrawString(_font, option2, new Vector2(300, 250), Color.White);
            spriteBatch.DrawString(_font, exit, new Vector2(300, 300), Color.White);
        }
    }
}
