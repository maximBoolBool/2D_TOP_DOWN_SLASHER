using UnityEngine;

namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))] // Гарантирует наличие Rigidbody2D
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        private Rigidbody2D _rb;

        private float _damage;
        private float _maxDistance;
        private Vector3 _spawnPosition;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float damage, float maxDistance, Vector2 direction)
        {
            _damage = damage;
            _maxDistance = maxDistance;
            _spawnPosition = transform.position;

            if (_rb != null)
            {
                // В Unity 6 — linearVelocity, в старых версиях — velocity
                _rb.linearVelocity = direction * speed;
            }
        }

        private void Update()
        {
            if (Vector3.Distance(_spawnPosition, transform.position) >= _maxDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) return;

            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<UnitCharecteristic>()?.SetDamage((int)_damage);
            }

            Destroy(gameObject);
        }
    }
}
