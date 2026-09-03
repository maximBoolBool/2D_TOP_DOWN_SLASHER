using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        private IWeapon _currentWeapon;
        private bool _isFirePressed;
        private float _nextFireTime;

        private void Awake()
        {
            if (playerInput == null) playerInput = GetComponentInParent<PlayerInput>();
            _currentWeapon = GetComponent<IWeapon>();
        }

        private void OnEnable()
        {
            if (playerInput != null)
            {
                playerInput.actions["Fire"].performed += OnFirePerformed;
                playerInput.actions["Fire"].canceled += OnFireCanceled;
            }
        }

        private void OnDisable()
        {
            if (playerInput != null)
            {
                playerInput.actions["Fire"].performed -= OnFirePerformed;
                playerInput.actions["Fire"].canceled -= OnFireCanceled;
            }
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            if (_currentWeapon == null) return;

            if (_currentWeapon.IsAutomatic)
            {
                _isFirePressed = true;
            }
            else
            {
                _currentWeapon.Execute();
            }
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            _isFirePressed = false;
        }

        private void Update()
        {
            if (_isFirePressed && _currentWeapon != null && _currentWeapon.IsAutomatic)
            {
                _currentWeapon.Execute();
            }
        }
    }
}
