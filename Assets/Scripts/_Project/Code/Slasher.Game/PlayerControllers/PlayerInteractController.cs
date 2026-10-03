using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers
{
    public class PlayerInteractController : MonoBehaviour
    {
        [Header("Unit references")]
        [SerializeField] private PlayerInput _playerInput;

        private InteractiveWrapper _interactiveWrapper;
        private InputAction _interactAction;

        public void SetInteractiveWrapper(InteractiveWrapper interactiveWrapper)
        {
            _interactiveWrapper = interactiveWrapper;
        }

        private void OnEnable()
        {
            if (_playerInput == null)
            {
                return;
            }

            _interactAction = _playerInput.actions[PlayerInputActionNames.INTERACT];
            _interactAction.started += OnInteract;
        }

        private void OnDisable()
        {
            // при выгрузке сцены PlayerInput может быть уничтожен раньше нас,
            // поэтому отписываемся через сохранённый action, а не через PlayerInput
            if (_interactAction != null)
            {
                _interactAction.started -= OnInteract;
                _interactAction = null;
            }
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (_interactiveWrapper != null)
            {
                _interactiveWrapper.Interact();
            }
        }
    }
}
