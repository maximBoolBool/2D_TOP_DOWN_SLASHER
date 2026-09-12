using UnityEngine;

namespace Assets.Scripts.Enemies
{
    internal class CaterpillarEnemy : BaseEnemy
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
            Debug.Log("CaterpillarEnemy executed");
        }
    }
}
