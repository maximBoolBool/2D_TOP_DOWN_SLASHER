using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public class AssaultRifle_1 : RangeWeapon
    {
        [Header("Spawn Settings")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint;

        public override void Execute()
        {
            if (bulletPrefab == null)
            {
                Debug.LogError("Ошибка: Не назначен bulletPrefab в инспекторе на объекте " + gameObject.name);
                return;
            }

            if (firePoint == null)
            {
                Debug.LogError("Ошибка: Не назначен firePoint в инспекторе на объекте " + gameObject.name);
                return;
            }

            var bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Vector2 fireDirection = firePoint.right;
            if (DeviationAngle > 0)
            {
                float randomOffset = Random.Range(-DeviationAngle / 2f, DeviationAngle / 2f);
                fireDirection = Quaternion.Euler(0f, 0f, randomOffset) * fireDirection;
            }

            bulletInstance.Initialize(Damage, Distance, fireDirection);
        }
    }
}
