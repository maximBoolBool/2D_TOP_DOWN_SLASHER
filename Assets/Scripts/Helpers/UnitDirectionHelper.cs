using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class UnitDirectionHelper
    {
        private const int ZERO_ANGLE_VALUE = 0;
        private const int HALF_CIRCLE_ANGLE_VALUE = 180;

        public static void SetDirection(GameObject unit, Vector2 direction)
        {
            if (direction == Vector2.zero) 
            {
                return;
            }
            var rotation = direction.x > 0 ? ZERO_ANGLE_VALUE : HALF_CIRCLE_ANGLE_VALUE;
            unit.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}
