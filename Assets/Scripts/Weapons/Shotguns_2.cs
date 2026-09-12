using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Shotguns_2 : RangeWeapon
    {
        [SerializeField]
        private int SubBulletsCount = 4;

        public override void Execute()
        {
            if (!mayFire)
            {
                return;
            }

            _ = BulletCreateHelper.InitializeBullets(
                bullet: bulletPrefab,
                firePoint: firePoint,
                count: SubBulletsCount,
                damage: Damage,
                distance: Distance,
                deviationAngle: DeviationAngle
            );

            mayFire = false;
            StartCoroutine(ResetFireCooldown());
        }
    }
}
