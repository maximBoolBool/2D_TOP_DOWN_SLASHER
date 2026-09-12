using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class BugEnemy : BaseEnemy
    {
        private void Awake()
        {
            BaseAwake();
        }

        private void FixedUpdate()
        {
            Execute();
        }

        public override void Execute()
        {
            if (!IsExecutionAvailable())
            {
                return;
            }

            MoveTowardsPlayer();
        }
    }
}
