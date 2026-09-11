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
            Debug.Log("FlyEnemy executed");
        }
    }
}
