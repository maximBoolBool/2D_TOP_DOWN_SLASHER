using Assets.Scripts.EnemyExecutionsStrategies;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyExecutionStrategyBehaviorus : MonoBehaviour
    {
        [SerializeField]
        private IEnemyExecutionStrategy _enemyExecutionStrategy;

        public void ExecuteStrategy()
        {
            _enemyExecutionStrategy.Execute();
        }
    }
}
