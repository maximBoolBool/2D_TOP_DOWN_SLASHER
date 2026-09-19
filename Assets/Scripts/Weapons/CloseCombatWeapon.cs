using Assets.Scripts.Constants;
using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class CloseCombatWeapon : Weapon
    {
        [field: SerializeField]
        public int Angle { get; set; }

        [field: SerializeField]
        public TargetType TargetType { get; set; }

        [field: SerializeField]
        public float ExecutionSpeed { get; set; }

        private const float radius = 1f;
        private Coroutine? _executionRoutine;

        public void Execute(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var angleFrom = angle - Angle;
            var angleTo = angle + Angle;

            if (_executionRoutine != null)
            {
                StopCoroutine(_executionRoutine);
            }

            _executionRoutine = StartCoroutine(ExecuteArc(angleFrom, angleTo));
        }

        // подумать над иерархией
        public override void Execute()
        {
            Execute(transform.right);
        }

        private IEnumerator ExecuteArc(float angleFrom, float angleTo)
        {
            float duration = 1 / ExecutionSpeed;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float currentAngle = Mathf.Lerp(angleFrom, angleTo, t);
                float radians = currentAngle * Mathf.Deg2Rad;

                var center = transform.parent != null ? transform.parent.position : transform.position;
                transform.position = center + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * radius;
                transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

                yield return null;
            }

            _executionRoutine = null;
            this.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(TagConstants.BULLET))
            {
                return;
            }

            if ((collider.gameObject.CompareTag(TagConstants.PLAYER) && TargetType == TargetType.Player)
                || (collider.gameObject.CompareTag(TagConstants.ENEMY) && TargetType == TargetType.Enemy)
            )
            {
                collider.gameObject.GetComponent<UnitCharecteristic>()?.SetDamage((int)Damage);
            }
        }
    }
}
