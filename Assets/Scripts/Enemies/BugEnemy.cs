using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class BugEnemy : BaseEnemy
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
            Debug.Log("BugEnemy executed");
        }
    }
}
