using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField]
        public string Title { get; set; }
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
        public float RateOfFire { get; set; } = 1f;

        [field: SerializeField]
        public int MagazineRounds { get; set; } = 1;

        [field: SerializeField]
        public int DeviationAngle { get; set; }

        [Header("Spawn Settings")]

        [field: SerializeField]
        protected Bullet bulletPrefab;
        
        [field: SerializeField]
        protected Transform firePoint;

        protected bool mayFire = true;

        protected IEnumerator ResetFireCooldown()
        {
            yield return new WaitForSeconds(RateOfFire);
            mayFire = true;
        }
    }
}
