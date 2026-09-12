using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyRabbit : BaseEnemy
    {
        private void Awake()
        {
            BaseAwake();
        }

        public override void Execute()
        {
            if (!IsExecutionAvailable())
            {
                return;
            }
            Debug.Log("EnemyRabbit executed");
        }
    }
}
