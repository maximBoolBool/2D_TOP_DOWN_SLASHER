using Assets.Scripts.Helpers;
using Assets.Scripts.Models;

namespace Assets.Scripts.Weapons
{
    public class AssaultRifle_1 : RangeWeapon
    {
        public override void Execute()
        {
            if (!mayFire)
            {
                return;
            }

            _ = BulletCreateHelper.InitializeBullets(
                bullet: bulletPrefab,
                firePoint: firePoint,
                count: 1,
                damage: Damage,
                distance: Distance,
                deviationAngle: DeviationAngle
            );

            mayFire = false;
            StartCoroutine(ResetFireCooldown());
        }
    }
}
