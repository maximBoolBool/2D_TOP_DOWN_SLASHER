using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.PlayerControllers
{
    public class PlayerWeaponAimController : MonoBehaviour
    {
        private Camera mainCamera;
        private SpriteRenderer _spriteRenderer;
        private PlayerInput _playerInput;
        private UnitCharecteristic _charecteristic;

        private void Awake()
        {
            mainCamera = Camera.main;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerInput = GetComponentInParent<PlayerInput>();
            _charecteristic = GetComponentInParent<UnitCharecteristic>();
        }

        private void OnEnable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Aim"].performed += OnAimPerformed;
            }
        }

        private void OnDisable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Aim"].performed -= OnAimPerformed;
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

            Vector3 aimDirection = (mouseWorldPosition - transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            _spriteRenderer.flipY = Mathf.Abs(angle) > 90f;
        }
    }
}
