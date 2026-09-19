using Assets.Scripts.Constants;
using Assets.Scripts.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.PlayerControllers
{
    public class PlayerCloseCombatWeaponController : MonoBehaviour
    {
        [SerializeField]
        private CloseCombatWeapon closeCombatWeaponPrefab;

        private CloseCombatWeapon? _closeCombatWeapon;
        private PlayerInput _playerInput;
        private Camera _mainCamera;

        private void Awake()
        {
            _playerInput = GetComponentInParent<PlayerInput>();
            _mainCamera = Camera.main;
            _closeCombatWeapon = Instantiate(closeCombatWeaponPrefab, transform.position, Quaternion.identity, transform);
            _closeCombatWeapon.gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            _playerInput.actions[PlayerInputActionNames.CLOSE_COMBAT_ACTION].performed += Execute;
        }

        public void OnDisable()
        {
            _playerInput.actions[PlayerInputActionNames.CLOSE_COMBAT_ACTION].performed -= Execute;
        }

        public void Execute(InputAction.CallbackContext context)
        {
            if(_closeCombatWeapon != null)
            {
                var mouseScreenPosition = _playerInput.actions[PlayerInputActionNames.AIM].ReadValue<Vector2>();
                var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
                var gameObjectPosition = transform.position;
                mouseWorldPosition.z = gameObjectPosition.z;

                _closeCombatWeapon.gameObject.SetActive(true);
                _closeCombatWeapon.Execute(mouseWorldPosition - gameObjectPosition);
            }
        }
    }
}
