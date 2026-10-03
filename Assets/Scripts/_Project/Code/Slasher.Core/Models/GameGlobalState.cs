using Assets.Scripts._Project.Code.Slasher.Core.Enums;

namespace Assets.Scripts._Project.Code.Slasher.Core.Models
{
    public interface IGameGlobalStateManager
    {
        GameGlobalStateType CurrentState { get; }
        string GameCurrentLevelName { get; }

        void SetStatus(GameGlobalStateType newState);
        void SetCurrentLevelName(string levelName);
    }

    public class GameGlobalState : IGameGlobalStateManager
    {
        public GameGlobalStateType CurrentState { get; private set; }

        public string GameCurrentLevelName { get; private set; }

        public void SetCurrentLevelName(string levelName)
        {
            GameCurrentLevelName = levelName;
        }

        public void SetStatus(GameGlobalStateType newState)
        {
            CurrentState = newState;
        }
    }
}
