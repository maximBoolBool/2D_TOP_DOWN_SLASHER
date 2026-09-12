using Assets.Scripts.Constants;
using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected UnitCharecteristic unitCharecteristic;
        protected GameObject player;

        [SerializeField] protected LayerMask obstacleLayer;
        [SerializeField] protected float obstacleCheckDistance = 1.5f;        

        public string Name { get; set; }
        public abstract void Execute();

        protected void BaseAwake()
        {
            unitCharecteristic = GetComponent<UnitCharecteristic>();
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER);
        }

        protected bool IsExecutionAvailable() => unitCharecteristic.IsAlive;

        protected void MoveTowardsPlayer()
        {
            if (player == null)
            {
                return;
            }

            Vector2 desiredDirection = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (!LineOfSightHelper.IsHasLineOfSight(transform.position, desiredDirection, distanceToPlayer, obstacleLayer))
            {
                return;
            }

            Vector2 finalDirection = AvoidObstacles(desiredDirection);

            Debug.Log(finalDirection);
            UnitDirectionHelper.SetDirection(gameObject, finalDirection);
            UnitAnimationHelper.SetAnimation(GetComponent<Animator>(), finalDirection);
            rb.MovePosition(rb.position + finalDirection * unitCharecteristic.ActualCharacteristics[CharecteristicType.Speed] * Time.fixedDeltaTime);
        }



        private Vector2 AvoidObstacles(Vector2 dir)
        {
            float[] angles = new float[] { 0, -30f, 30f, -60f, 60f };

            foreach (float angle in angles)
            {
                Vector2 rayDir = Quaternion.Euler(0, 0, angle) * dir;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, obstacleCheckDistance, obstacleLayer);

                if (hit.collider == null)
                {
                    return rayDir;
                }
            }

            return dir;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector2 dir = player != null ? (Vector2)(player.transform.position - transform.position).normalized : (Vector2)transform.up;
            Gizmos.DrawRay(transform.position, dir * obstacleCheckDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -30) * dir * obstacleCheckDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, 30) * dir * obstacleCheckDistance);
        }
    }
}
