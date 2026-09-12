using UnityEngine;

namespace Assets.Scripts.Models
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField]
        public string Tittle { get; set; }
        [field: SerializeField]
        public float Damage { get; set; }

        public abstract void Execute();
    }

    public abstract class RangeWeapon : Weapon
    {
        [Header("Weapon Properties")]
        [field: SerializeField]
        public bool IsAutomatic { get; set; }
        [field: SerializeField]
        public float Distance { get; set; }
        [field: SerializeField]
        public float? RateOfFire { get; set; }        
        [field: SerializeField] 
        public int? MagazineRounds { get; set; }
        [field: SerializeField]
        public int DeviationAngle { get; set; }
    }
}
