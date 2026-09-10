using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class UnitAnimationHelper
    {
        private const string IdleMovingTrigger = "IdleMovingTrigger";
        private const string MovingIdleTrigger = "MovingIdleTrigger";
        private const string DeadTrigger = "DeadTrigger";

        public static void SetAnimationDirection(Animator animator, Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                Debug.Log($"ZZ {Vector2.zero}");
                animator.ResetTrigger(IdleMovingTrigger);
                animator.SetTrigger(MovingIdleTrigger);
                return;
            }

            animator.ResetTrigger(MovingIdleTrigger);
            animator.SetTrigger(IdleMovingTrigger);
        }

        public static void SetDeadAnimation(Animator animator)
        {
            animator.SetTrigger(DeadTrigger);
        }
    }
}

