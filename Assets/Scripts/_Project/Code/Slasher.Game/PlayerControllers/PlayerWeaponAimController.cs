using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers
{
    public class PlayerWeaponAimController : MonoBehaviour
    {
        [Header("Unit references")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private UnitCharecteristic _charecteristic;
        [Tooltip("Объект, который поворачивается за прицелом (AimShootUnitPart)")]
        [SerializeField] private Transform _aimRoot;

        private Camera mainCamera;
        private SpriteRenderer _spriteRenderer;
        private InputAction _aimAction;

        private void Awake()
        {
            mainCamera = Camera.main;
            _spriteRenderer = _aimRoot.GetComponentInChildren<SpriteRenderer>();
        }

        public void RefreshSprite()
        {
            _spriteRenderer = _aimRoot.GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (_playerInput == null)
            {
                return;
            }

            _aimAction = _playerInput.actions[PlayerInputActionNames.AIM];
            _aimAction.performed += OnAimPerformed;
        }

        private void OnDisable()
        {
            // при выгрузке сцены PlayerInput может быть уничтожен раньше нас,
            // поэтому отписываемся через сохранённый action, а не через PlayerInput
            if (_aimAction != null)
            {
                _aimAction.performed -= OnAimPerformed;
                _aimAction = null;
            }
        }

        private void OnAimPerformed(InputAction.CallbackContext context)
        {
            if (!_charecteristic.IsAlive)
            {
                return;
            }

            Vector2 mouseScreenPosition = context.ReadValue<Vector2>();

            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0f;

            var aimDirection = (mouseWorldPosition - _aimRoot.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            _aimRoot.rotation = Quaternion.Euler(0f, 0f, angle);
            _spriteRenderer.flipY = Mathf.Abs(angle) > 90f;
        }
    }
}
