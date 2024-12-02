using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PingPongGame.Managers;
using PingPongGame.States;

namespace PingPongGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameStateManager _gameStateManager;
    private InputManager _inputManager;
    private MenuState _menuState;
    private PlayState _playState;

    private SpriteFont _font;
    private Song _backgroundMusic;
    private int _screenWidth;
    private int _screenHeight;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        _screenWidth = _graphics.PreferredBackBufferWidth = 800;
        _screenHeight = _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();

        _gameStateManager = new GameStateManager();
        _inputManager = new InputManager();
        _menuState = new MenuState(_gameStateManager);
        _playState = new PlayState(_gameStateManager, false, _screenWidth, _screenHeight);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _menuState.LoadContent(Content);
        _font = Content.Load<SpriteFont>("font");
        _playState.LoadContent(Content);

        _backgroundMusic = Content.Load<Song>("Sounds/backgroundMusic");

        MediaPlayer.Play(_backgroundMusic);
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.1f;
    }

    protected override void Update(GameTime gameTime)
    {
        _inputManager.Update();

        switch (_gameStateManager.CurrentState)
        {
            case GameState.Menu:
                if (_playState.IsPlaying)
                {
                    _playState.StopGame();
                }
                _menuState.Update(_inputManager);
                break;
            case GameState.Play:
                if (!_playState.IsPlaying)
                {
                    _playState.StartGame(_gameStateManager.CurrentMode == GameMode.TwoPlayer);
                }
                _playState.Update(gameTime, _inputManager);
                break;
            case GameState.Exit:
                Exit();
                break;
        }

        // Toggle music on/off with 'M'
        if (_inputManager.IsKeyPressed(Keys.M))
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
            else
                MediaPlayer.Resume();
        }

        // Adjust volume with '+' and '-'
        if (_inputManager.IsKeyPressed(Keys.OemPlus) && MediaPlayer.Volume < 1.0f)
            MediaPlayer.Volume += 0.1f;
        if (_inputManager.IsKeyPressed(Keys.OemMinus) && MediaPlayer.Volume > 0.0f)
            MediaPlayer.Volume -= 0.1f;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        switch (_gameStateManager.CurrentState)
        {
            case GameState.Menu:
                _menuState.Draw(_spriteBatch);
                break;
            case GameState.Play:
                _playState.Draw(_spriteBatch);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
