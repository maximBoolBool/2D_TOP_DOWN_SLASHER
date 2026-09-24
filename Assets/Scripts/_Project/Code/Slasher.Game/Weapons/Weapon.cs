using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Game.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField]
        public string Title { get; set; }
        [field: SerializeField]
        public float Damage { get; set; }

        public abstract void Execute();
    }
}
