using Assets.Scripts.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.PlayerControllers
{
    public class PlayerCloseCombatWeaponController : MonoBehaviour
    {
        [SerializeField]
        private CloseCombatWeapon? _closeCombatWeapon;
        private PlayerInput _playerInput;
        private Camera _mainCamera;

        private void Awake()
        {
            _playerInput = GetComponentInParent<PlayerInput>();
            _mainCamera = Camera.main;
        }

        public void OnEnable()
        {
            _playerInput.actions["CloseCombatAction"].performed += Execute;
        }

        public void OnDisable()
        {
            _playerInput.actions["CloseCombatAction"].performed -= Execute;
        }

        public void Execute(InputAction.CallbackContext context)
        {
            if(_closeCombatWeapon != null)
            {
                var mouseScreenPosition = _playerInput.actions["Aim"].ReadValue<Vector2>();
                var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
                var gameObjectPosition = transform.position;
                mouseWorldPosition.z = gameObjectPosition.z;

                _closeCombatWeapon.Execute(mouseWorldPosition - gameObjectPosition);
            }
        }
    }
}
