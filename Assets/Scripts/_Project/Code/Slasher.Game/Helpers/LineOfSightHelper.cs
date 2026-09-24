using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Game.Helpers
{
    public static class LineOfSightHelper
    {
        public static bool IsHasLineOfSight(
            Vector3 start,
            Vector2 direction,
            float distance,
            LayerMask obstacleLayer
        )
        {
            var losHit = Physics2D.Raycast(start, direction, distance, obstacleLayer);
            return losHit.collider == null;
        }
    }
}
