using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic;
using Assets.Scripts._Project.Code.Slasher.Game.Services;
using Assets.Scripts._Project.Code.Slasher.Game.Weapons;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers
{
    public class PlayerShootController : MonoBehaviour
    {
        [Inject]
        private IWeaponUILoadService _weaponUILoadService;

        private UnitCharecteristic unitCharecteristic;
        private RangeWeapon[] _weapons;
        private PlayerInput playerInput;
        private RangeWeapon[] AutomaticWeapon => _weapons.Where(w => w.IsAutomatic).ToArray();
        private RangeWeapon CurrentWeapon => _weapons.FirstOrDefault();

        private void Awake()
        {
            playerInput = GetComponentInParent<PlayerInput>();
            unitCharecteristic = GetComponentInParent<UnitCharecteristic>();
            _weapons = GetComponentsInChildren<RangeWeapon>();
        }

        private void Start()
        {
            _weaponUILoadService?.SetWeapon(CurrentWeapon);
        }

        public void RefreshWeapons()
        {
            _weapons = GetComponentsInChildren<RangeWeapon>();
            _weaponUILoadService?.SetWeapon(CurrentWeapon);
        }

        private void OnEnable()
        {
            if (playerInput != null)
            {
                playerInput.actions[PlayerInputActionNames.FIRE].performed += OnFirePerformed;
                playerInput.actions[PlayerInputActionNames.FIRE].canceled += OnFireCanceled;
                playerInput.actions[PlayerInputActionNames.RELOAD].started += OnReload;
            }
        }

        private void OnDisable()
        {
            if (playerInput != null)
            {
                playerInput.actions[PlayerInputActionNames.FIRE].performed -= OnFirePerformed;
                playerInput.actions[PlayerInputActionNames.FIRE].canceled -= OnFireCanceled;
                playerInput.actions[PlayerInputActionNames.RELOAD].started -= OnReload;
            }

            CancelInvoke(nameof(AutomaticFireTick));
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            if(!unitCharecteristic.IsAlive)
            {
                return;
            }

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

        private void OnReload(InputAction.CallbackContext context)
        {
            if (!unitCharecteristic.IsAlive)
            {
                return;
            }

            foreach (var weapon in _weapons)
            {
                weapon.Reload();
            }
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
                // как в ETG: нажал на спуск с пустым магазином — началась перезарядка
                if (!weapon.TryShoot() && weapon.AmmoInMagazine == 0)
                {
                    weapon.Reload();
                }
            }
        }
    }
}
