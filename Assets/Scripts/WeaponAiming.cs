using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class WeaponAiming : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        private SpriteRenderer _spriteRenderer;
        private PlayerInput _playerInput;

        private void Awake()
        {
            if (mainCamera == null) mainCamera = Camera.main;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerInput = GetComponentInParent<PlayerInput>();
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
