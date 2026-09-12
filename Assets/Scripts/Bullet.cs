using Assets.Scripts.Constants;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private TargetType _sideType = TargetType.All;
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
            Debug.LogWarning($"Bullet collided with {collision.gameObject.name}");

            if (collision.gameObject.CompareTag(TagConstants.PLAYER))
            {
                return;
            }

            if (collision.gameObject.CompareTag(TagConstants.ENEMY))
            {
                collision.gameObject.GetComponent<UnitCharecteristic>()?.SetDamage((int)_damage);
            }

            Destroy(gameObject);
        }
    }
}