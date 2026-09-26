using Assets.Scripts._Project.Code.Slasher.Core.Enums;

namespace Assets.Scripts._Project.Code.Slasher.Core.Models
{
    public interface IGameGlobalStateManager
    {
        GameGlobalStateType CurrentState { get; }

        void SetStatus(GameGlobalStateType newState);
    }

    public class GameGlobalState : IGameGlobalStateManager
    {
        public GameGlobalStateType CurrentState { get; private set; }

        public void SetStatus(GameGlobalStateType newState)
        {
            CurrentState = newState;
        }
    }
}
