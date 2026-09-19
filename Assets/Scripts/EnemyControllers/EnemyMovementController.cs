using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.EnemyControllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovementController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _animator;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void MoveTowards(Vector3 targetPosition, float speed, float obstacleCheckDistance, LayerMask obstacleLayer)
        {
            Vector2 desiredDirection = (targetPosition - transform.position).normalized;
            float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

            if (!LineOfSightHelper.IsHasLineOfSight(transform.position, desiredDirection, distanceToTarget, obstacleLayer))
            {
                Stop();
                return;
            }

            Vector2 finalDirection = AvoidObstacles(desiredDirection, obstacleCheckDistance, obstacleLayer);

            UnitDirectionHelper.SetDirection(gameObject, finalDirection);
            UnitAnimationHelper.SetAnimation(_animator, finalDirection);

            _rb.MovePosition(_rb.position + finalDirection * speed * Time.fixedDeltaTime);
        }

        public void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
            if (_animator != null)
            {
                UnitAnimationHelper.SetAnimation(_animator, Vector2.zero);
            }
        }

        private Vector2 AvoidObstacles(Vector2 dir, float distance, LayerMask layer)
        {
            float[] angles = new float[] { 0, -30f, 30f, -60f, 60f };

            foreach (float angle in angles)
            {
                Vector2 rayDir = Quaternion.Euler(0, 0, angle) * dir;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, distance, layer);

                if (hit.collider == null)
                {
                    return rayDir;
                }
            }

            return dir;
        }
    }
}
