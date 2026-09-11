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
            Debug.Log("EnemyRabbit executed");
        }
    }
}
