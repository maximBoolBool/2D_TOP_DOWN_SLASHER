using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class RangeWeapon : Weapon
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

        [field: SerializeField]
        public WeaponType Type { get; set; }

        [field: SerializeField]
        public int BulletsPerShot { get; set; } = 1;

        [Header("Spawn Settings")]

        [field: SerializeField]
        protected Bullet bulletPrefab;

        [field: SerializeField]
        protected Transform firePoint;

        protected bool mayFire = true;

        private ParticleSystem _shellParticleSystem;

        public override void Execute()
        {
            if (!mayFire)
            {
                return;
            }

            _ = BulletCreateHelper.InitializeBullets(
                bullet: bulletPrefab,
                firePoint: firePoint,
                count: BulletsPerShot,
                damage: Damage,
                distance: Distance,
                deviationAngle: DeviationAngle
            );

            mayFire = false;
            EjectShell();
            StartCoroutine(ResetFireCooldown());
        }

        protected IEnumerator ResetFireCooldown()
        {
            yield return new WaitForSeconds(RateOfFire);
            mayFire = true;
        }

        protected void EjectShell()
        {
            if (_shellParticleSystem != null)
            {
                _shellParticleSystem.Emit(1);
            }
        }

        private void Awake()
        {
            _shellParticleSystem = GetComponentInChildren<ParticleSystem>();
        }
    }
}
