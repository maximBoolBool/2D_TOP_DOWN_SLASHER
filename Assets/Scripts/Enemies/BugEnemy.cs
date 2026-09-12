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
            Debug.LogWarning($"FixedUpdate");
            Execute();
        }

        public override void Execute()
        {
            if (!IsExecutionAvailable())
            {
                Debug.LogWarning($"1");
                return;
            }

            Debug.LogWarning($"2");
            MoveTowardsPlayer();
        }
    }
}
