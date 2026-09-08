using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        private IRangeWeapon _currentWeapon;

        private void Awake()
        {
            if (playerInput == null) playerInput = GetComponentInParent<PlayerInput>();
            _currentWeapon = GetComponent<IRangeWeapon>();
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

            CancelInvoke(nameof(FireTick));
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            if (_currentWeapon == null) return;

            if (_currentWeapon.IsAutomatic && _currentWeapon.RateOfFire != null)
            {
                // первый выстрел сразу, дальше — по таймеру
                FireTick();
                InvokeRepeating(
                    nameof(FireTick),
                    _currentWeapon.RateOfFire.Value,
                    _currentWeapon.RateOfFire.Value
                );
            }
            else
            {
                _currentWeapon.Execute();
            }
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            CancelInvoke(nameof(FireTick));
        }

        private void FireTick()
        {
            _currentWeapon?.Execute();
        }
    }
}
