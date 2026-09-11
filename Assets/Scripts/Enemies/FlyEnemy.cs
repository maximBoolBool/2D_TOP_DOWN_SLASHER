using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class FlyEnemy : BaseEnemy
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
            Debug.Log("FlyEnemy executed");
        }
    }
}
