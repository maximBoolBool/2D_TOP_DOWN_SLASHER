using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers
{
    public class PlayerCloseCombatWeaponController : MonoBehaviour
    {
        [SerializeField]
        private CloseCombatWeapon closeCombatWeaponPrefab;

        private CloseCombatWeapon _closeCombatWeapon;
        private PlayerInput _playerInput;
        private Camera _mainCamera;
        private InputAction _closeCombatAction;

        private void Awake()
        {
            _playerInput = GetComponentInParent<PlayerInput>();
            _mainCamera = Camera.main;
            _closeCombatWeapon = Instantiate(closeCombatWeaponPrefab, transform.position, Quaternion.identity, transform);
            _closeCombatWeapon.gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            if (_playerInput == null)
            {
                return;
            }

            _closeCombatAction = _playerInput.actions[PlayerInputActionNames.CLOSE_COMBAT_ACTION];
            _closeCombatAction.performed += Execute;
        }

        public void OnDisable()
        {
            // при выгрузке сцены PlayerInput может быть уничтожен раньше нас,
            // поэтому отписываемся через сохранённый action, а не через PlayerInput
            if (_closeCombatAction != null)
            {
                _closeCombatAction.performed -= Execute;
                _closeCombatAction = null;
            }
        }

        public void Execute(InputAction.CallbackContext context)
        {
            if (_closeCombatWeapon != null)
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
