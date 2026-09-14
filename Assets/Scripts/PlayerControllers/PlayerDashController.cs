using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.PlayerControllers
{
    public class PlayerDashController : MonoBehaviour
    {
        [SerializeField] private float dashDistance = 0.001f;
        [SerializeField] private float dashDuration = 0.4f;
        [SerializeField] private float dashCooldown = 1f;
        private Rigidbody2D _rb;
        private PlayerInput _playerInput;
        private InputAction _aimAction;
        private Camera _mainCamera;
        private bool _isDashing;
        private bool _canDash = true;
        private UnitCharecteristic _unitCharecteristic;

        public bool IsDashing => _isDashing;

        private void Awake()
        {
            _unitCharecteristic = GetComponent<UnitCharecteristic>();
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
            if(!_unitCharecteristic.IsAlive || !_canDash)
            {
                return;
            }

            var mouseScreenPosition = _aimAction.ReadValue<Vector2>();
            var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            var gameObjectPosition = transform.position;
            mouseWorldPosition.z = gameObjectPosition.z;
            Vector2 dashDirection = (mouseWorldPosition - gameObjectPosition).normalized;
            StartCoroutine(PerformDash(dashDirection));
        }

        private IEnumerator PerformDash(Vector2 direction)
        {
            if (_isDashing)
            {
                yield break;
            }

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

            _rb.linearVelocity = Vector2.zero;

            _isDashing = false;
            yield return new WaitForSeconds(dashCooldown);

            _canDash = true;
        }
    }
}
