using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyRabbit : BaseEnemy
    {
        public void Awake()
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
