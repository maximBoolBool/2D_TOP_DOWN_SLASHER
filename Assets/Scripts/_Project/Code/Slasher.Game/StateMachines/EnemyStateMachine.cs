using Assets.Scripts._Project.Code.Slasher.Game.States;

namespace Assets.Scripts._Project.Code.Slasher.Game.StateMachines
{
    public class EnemyStateMachine
    {
        public IState CurrentState { get; private set; }

        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(IState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
