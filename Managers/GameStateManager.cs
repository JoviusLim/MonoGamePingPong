using Microsoft.Xna.Framework;

namespace PingPongGame.Managers
{
    public enum GameState
    {
        Menu,
        Play,
        Exit
    }

    public enum GameMode
    {
        OnePlayer,
        TwoPlayer
    }

    public class GameStateManager
    {
        public GameState CurrentState { get; private set; }
        public GameMode CurrentMode { get; private set; }
        public GameStateManager()
        {
            CurrentState = GameState.Menu;
        }

        public void ChangeState(GameState newState) => CurrentState = newState;
        public void ChangeMode(GameMode newMode) => CurrentMode = newMode;
    }
}