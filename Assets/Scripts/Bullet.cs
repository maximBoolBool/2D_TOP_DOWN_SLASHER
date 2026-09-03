using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private Rigidbody2D rb;

        private float _damage;
        private float _maxDistance;
        private Vector3 _spawnPosition;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float damage, float maxDistance, Vector2 direction)
        {
            _damage = damage;
            _maxDistance = maxDistance;
            _spawnPosition = transform.position;

            // Поворачиваем спрайт пули в сторону полёта
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Задаем физическую скорость
            rb.linearVelocity = direction * speed;
        }

        private void Update()
        {
            // Удаляем пулю, если она пролетела дальше заданной дистанции
            if (Vector3.Distance(_spawnPosition, transform.position) >= _maxDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Проверка на попадание во врага/препятствие
            // if (collision.TryGetComponent<IDamageable>(out var target)) target.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }
}
