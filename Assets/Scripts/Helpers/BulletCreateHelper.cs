using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class BulletCreateHelper
    {
        public static List<Bullet> InitializeBullets(
            Bullet bullet,
            Transform firePoint,
            int count,
            float damage,
            float distance,
            float deviationAngle
        )
        {
            var bullets = new List<Bullet>();

            for (int i = 0; i < count; i++)
            {
                var bulletInstance = GameObject.Instantiate(bullet, firePoint.position, firePoint.rotation);
                bullets.Add(bulletInstance);
            }

            var bulletsFireDirections = new Dictionary<int, Vector2>();

            int[] sectorIndices = Enumerable.Range(0, bullets.Count).OrderBy(x => Random.value).ToArray();

            for (int i = 0; i < bullets.Count; i++)
            {
                Vector2 fireDirection = firePoint.right;

                float sectorSize = deviationAngle / bullets.Count;
                int sector = sectorIndices[i];
                float sectorStart = -deviationAngle / 2f + sector * sectorSize;
                float sectorEnd = sectorStart + sectorSize;

                float randomOffset = Random.Range(sectorStart, sectorEnd);

                fireDirection = Quaternion.Euler(0f, 0f, randomOffset) * fireDirection;
                bulletsFireDirections.Add(i, fireDirection);
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                Vector2 fireDirection = bulletsFireDirections[i];
                bullets[i].Initialize(damage, distance, fireDirection);
            }

            return bullets;
        }   
    }
}
