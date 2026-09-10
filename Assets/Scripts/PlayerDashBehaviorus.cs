using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerDashBehaviorus : MonoBehaviour
    {
        [SerializeField] private float dashDistance = 0.001f;    // всего 2 метра — короткий
        [SerializeField] private float dashDuration = 0.4f;  // растянут на 0.4 сек — не резкий рывок
        [SerializeField] private float dashCooldown = 1f;
        private Rigidbody2D _rb;
        private PlayerInput _playerInput;
        private InputAction _aimAction;
        private Camera _mainCamera;
        private bool _isDashing;
        private bool _canDash = true;
        public bool IsDashing => _isDashing;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            _aimAction = _playerInput.actions["Aim"];
        }

        private void OnEnable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Dash"].started += OnDash;
            }
        }

        private void OnDisable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Dash"].started -= OnDash;
            }
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            var mouseScreenPosition = _aimAction.ReadValue<Vector2>();
            var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            var gameObjectPosition = transform.position;
            mouseWorldPosition.z = gameObjectPosition.z;
            Vector2 dashDirection = (mouseWorldPosition - gameObjectPosition).normalized;
            StartCoroutine(PerformDash(dashDirection));
        }

        private IEnumerator PerformDash(Vector2 direction)
        {
            _isDashing = true;
            _canDash = false;

            float speed = dashDistance / dashDuration;
            float timer = 0f;

            while (timer < dashDuration)
            {
                _rb.linearVelocity = direction * speed;
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            _rb.linearVelocity = Vector2.zero; // резкая остановка после рывка
            _isDashing = false;

            yield return new WaitForSeconds(dashCooldown - dashDuration);
            _canDash = true;
        }
    }
}
