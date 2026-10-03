using Assets.Scripts._Project.Code.Slasher.Game.Enums;
using Assets.Scripts._Project.Code.Slasher.Game.Helpers;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Game.Weapons
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

        [Header("Ammo")]

        [field: SerializeField]
        public int MaxAmmo { get; set; } = 60;

        [field: SerializeField]
        public bool IsInfiniteAmmo { get; set; }

        [field: SerializeField]
        public float ReloadTime { get; set; } = 1f;

        [field: SerializeField]
        public Sprite AmmoIcon { get; set; }

        [Header("Spawn Settings")]

        [field: SerializeField]
        protected Bullet bulletPrefab;

        [field: SerializeField]
        protected Transform firePoint;

        protected bool mayFire = true;

        private ParticleSystem _shellParticleSystem;

        public int AmmoInMagazine { get; private set; }
        public int AmmoInReserve { get; private set; }
        public bool IsReloading { get; private set; }

        public bool CanShoot => mayFire && !IsReloading && AmmoInMagazine > 0;
        public bool CanReload => !IsReloading
            && AmmoInMagazine < MagazineRounds
            && (IsInfiniteAmmo || AmmoInReserve > 0);

        public event Action AmmoChanged;
        public event Action<float> ReloadStarted;

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

        public bool TryShoot()
        {
            if (!CanShoot)
            {
                return false;
            }

            Execute();
            AmmoInMagazine--;
            AmmoChanged?.Invoke();
            return true;
        }

        public void Reload()
        {
            if (!CanReload)
            {
                return;
            }

            StartCoroutine(ReloadRoutine());
        }

        private IEnumerator ReloadRoutine()
        {
            IsReloading = true;
            ReloadStarted?.Invoke(ReloadTime);

            yield return new WaitForSeconds(ReloadTime);

            int needed = MagazineRounds - AmmoInMagazine;
            int loaded = IsInfiniteAmmo ? needed : Mathf.Min(needed, AmmoInReserve);

            AmmoInMagazine += loaded;
            if (!IsInfiniteAmmo)
            {
                AmmoInReserve -= loaded;
            }

            IsReloading = false;
            AmmoChanged?.Invoke();
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
            AmmoInMagazine = MagazineRounds;
            AmmoInReserve = MaxAmmo;
        }

        private void OnDisable()
        {
            // корутины останавливаются при выключении объекта — сбрасываем флаги,
            // иначе оружие "зависнет" в перезарядке или кулдауне
            IsReloading = false;
            mayFire = true;
        }
    }
}
