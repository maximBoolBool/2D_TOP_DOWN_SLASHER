using Assets.Scripts.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.PlayerControllers
{
    public class PlayerShootController : MonoBehaviour
    {
        private PlayerInput playerInput;

        private RangeWeapon[] _weapons;

        private RangeWeapon[] AutomaticWeapon => _weapons.Where(w => w.IsAutomatic).ToArray();

        private void Awake()
        {
            playerInput = GetComponentInParent<PlayerInput>();
            _weapons = GetComponentsInChildren<RangeWeapon>();
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
            if (AutomaticWeapon.Any())
            {
                var minRateOfFire = AutomaticWeapon.Min(w => w.RateOfFire);
                FireTick(false);
                InvokeRepeating(
                    nameof(AutomaticFireTick),
                    minRateOfFire,
                    minRateOfFire
                );
            }
            else
            {
                FireTick(false);
            }
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            CancelInvoke(nameof(AutomaticFireTick));
        }

        private void AutomaticFireTick()
        {
            FireTick(true);
        }

        private void FireTick(bool onlyAutomatic)
        {
            var weapons = onlyAutomatic ? AutomaticWeapon : _weapons;

            foreach (var weapon in weapons)
            {
                weapon.Execute();
            }
        }
    }
}
