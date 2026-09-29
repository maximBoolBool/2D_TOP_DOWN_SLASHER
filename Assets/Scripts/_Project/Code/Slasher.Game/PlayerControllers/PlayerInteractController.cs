using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers
{
    public class PlayerInteractController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InteractiveWrapper _interactiveWrapper;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        public void SetInteractiveWrapper(InteractiveWrapper interactiveWrapper)
        {
            _interactiveWrapper = interactiveWrapper;
        }

        private void OnEnable()
        {
            _playerInput.actions[PlayerInputActionNames.INTERACT].started += OnInteract;
        }

        private void OnDisable()
        {
            _playerInput.actions[PlayerInputActionNames.INTERACT].started -= OnInteract;

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
