using UnityEngine;

namespace Assets.Scripts.Weapons
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
