using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Game.Interactables
{
    public class LevelGate : BaseInteractiveItem
    {
        private Animator _animator;
        private bool _isOpened;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();

            if (_animator == null)
            {
                Debug.LogError($"{nameof(LevelGate)}: Animator не найден в дочерних объектах {gameObject.name}", this);
            }
        }

        public override void Interact()
        {
            if (_isOpened)
            {
                Debug.Log($"{nameof(LevelGate)}: ворота {gameObject.name} уже открыты", this);
                return;
            }

            _isOpened = true;

            if (_animator != null)
            {
                _animator.SetTrigger(LevelGateAnimatorConstants.Open);
            }

            Debug.Log($"{nameof(LevelGate)}: ворота {gameObject.name} открываются", this);
        }
    }

    public static class LevelGateAnimatorConstants
    {
        public const string Open = "Open";
    }
}
