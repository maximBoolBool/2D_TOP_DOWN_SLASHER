using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public class RangedWeapon1 : MonoBehaviour, IWeapon
    {
        [Header("Weapon Interface Properties")]
        [field: SerializeField] public bool IsAutomatic { get; set; } = false;
        [field: SerializeField] public float Damage { get; set; } = 10f;
        [field: SerializeField] public float Distance { get; set; } = 15f;
        [field: SerializeField] public int DeviationAngle { get; set; } = 5; // Разброс в градусах

        [Header("Spawn Settings")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint; // Пустой GameObject на кончике дула

        public void Execute()
        {
            if (bulletPrefab == null || firePoint == null)
            {
                Debug.LogWarning("Не назначен bulletPrefab или firePoint!");
                return;
            }

            // 1. Получаем базовое направление выстрела (куда смотрит firePoint)
            Vector2 fireDirection = firePoint.right;

            // 2. Добавляем разброс (DeviationAngle)
            if (DeviationAngle > 0)
            {
                float randomOffset = UnityEngine.Random.Range(-DeviationAngle / 2f, DeviationAngle / 2f);
                fireDirection = Quaternion.Euler(0f, 0f, randomOffset) * fireDirection;
            }

            // 3. Инстанцируем пулю в точке firePoint
            Bullet bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // 4. Передаем параметры из IWeapon в пулю
            bulletInstance.Initialize(Damage, Distance, fireDirection);
        }
    }
}
