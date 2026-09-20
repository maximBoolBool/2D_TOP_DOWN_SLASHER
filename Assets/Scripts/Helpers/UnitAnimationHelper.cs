using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class UnitAnimationHelper
    {
        //Triggers
        public const string IdleMovingTrigger = "IdleMovingTrigger";
        public const string MovingIdleTrigger = "MovingIdleTrigger";
        public const string DeadTrigger = "DeadTrigger";
        public const string HitTrigger = "HitTrigger";

        //Layers
        public const string BaseLayerName = "BaseLayer";
        public const string HitLayer = "HitLayer";

        public static void SetAnimation(Animator animator, Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
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

        public static void SetDamagedAnimation(Animator animator)
        {
            int layerIndex = animator.GetLayerIndex(HitLayer);
            animator.SetLayerWeight(layerIndex, 1f);
            animator.SetTrigger(HitTrigger);
        }
    }
}

