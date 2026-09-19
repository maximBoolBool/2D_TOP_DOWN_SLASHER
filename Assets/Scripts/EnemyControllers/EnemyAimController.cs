using UnityEngine;

namespace Assets.Scripts.EnemyControllers
{
    public class EnemyAimController : MonoBehaviour
    {
        public float CurrentAimAngle { get; private set; }

        public void AimAt(Vector3 targetPosition, Transform weaponTransform)
        {
            Vector3 aimDirection = (targetPosition - transform.position).normalized;
            CurrentAimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            if (weaponTransform != null)
            {
                weaponTransform.rotation = Quaternion.Euler(0f, 0f, CurrentAimAngle);
            }
        }
    }
}
