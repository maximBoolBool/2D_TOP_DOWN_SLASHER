using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers
{
    public class PlayerDashController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float dashDistance = 0.001f;
        [SerializeField] private float dashDuration = 0.4f;
        [SerializeField] private float dashCooldown = 1f;

        [Header("Unit references")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private UnitCharecteristic _unitCharecteristic;
        [SerializeField] private PlayerMovementController _playerMovementController;

        private InputAction _aimAction;
        private InputAction _dashAction;
        private Camera _mainCamera;
        private bool _isDashing;
        private bool _canDash = true;

        public bool IsDashing => _isDashing;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _aimAction = _playerInput.actions[PlayerInputActionNames.AIM];
        }

        private void OnEnable()
        {
            if (_playerInput == null)
            {
                return;
            }

            _dashAction = _playerInput.actions[PlayerInputActionNames.DASH];
            _dashAction.started += OnDash;
        }

        private void OnDisable()
        {
            // при выгрузке сцены PlayerInput может быть уничтожен раньше нас,
            // поэтому отписываемся через сохранённый action, а не через PlayerInput
            if (_dashAction != null)
            {
                _dashAction.started -= OnDash;
                _dashAction = null;
            }
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            if (!_unitCharecteristic.IsAlive || !_canDash)
            {
                return;
            }

            var mouseScreenPosition = _aimAction.ReadValue<Vector2>();
            var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            var unitPosition = _rb.transform.position;
            mouseWorldPosition.z = unitPosition.z;
            Vector2 dashDirection = (mouseWorldPosition - unitPosition).normalized;
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
            _playerMovementController.RestoreMovement();

            _isDashing = false;
            yield return new WaitForSeconds(dashCooldown);

            _canDash = true;
        }
    }
}
