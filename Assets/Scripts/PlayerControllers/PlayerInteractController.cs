using Assets.Scripts.Constants;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.PlayerControllers
{
    public class PlayerInteractController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InteractiveWrapper? _interactiveWrapper;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        public void SetInteractiveWrapper(InteractiveWrapper? interactiveWrapper)
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
