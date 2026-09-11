using UnityEngine;

namespace Assets.Scripts.Enemies
{
    internal class CaterpillarEnemy : BaseEnemy
    {
        public void Awake()
        {
            BaseAwake();
        }

        public override void Execute()
        {
            Debug.Log("CaterpillarEnemy executed");
        }
    }
}
