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
            Debug.Log("BugEnemy executed");
        }
    }
}
