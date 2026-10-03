using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using Assets.Scripts._Project.Code.Slasher.Game.Weapons;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers
{
    public class PlayerShootController : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Header("Unit references")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private UnitCharecteristic unitCharecteristic;
        [Tooltip("Объект, внутри которого лежит оружие (AimShootUnitPart)")]
        [SerializeField] private Transform _weaponsRoot;

        private RangeWeapon[] _weapons;
        private InputAction _fireAction;
        private InputAction _reloadAction;
        private RangeWeapon[] AutomaticWeapon => _weapons.Where(w => w.IsAutomatic).ToArray();
        private RangeWeapon CurrentWeapon => _weapons.FirstOrDefault();

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _weapons = _weaponsRoot.GetComponentsInChildren<RangeWeapon>();
        }

        private void Start()
        {
            NotifyWeaponChanged();
        }

        public void RefreshWeapons()
        {
            _weapons = _weaponsRoot.GetComponentsInChildren<RangeWeapon>();
            NotifyWeaponChanged();
        }

        private void NotifyWeaponChanged()
        {
            _signalBus.Fire(new PlayerWeaponChangedMessage(CurrentWeapon));
        }

        private void OnEnable()
        {
            if (playerInput == null)
            {
                return;
            }

            _fireAction = playerInput.actions[PlayerInputActionNames.FIRE];
            _reloadAction = playerInput.actions[PlayerInputActionNames.RELOAD];

            _fireAction.performed += OnFirePerformed;
            _fireAction.canceled += OnFireCanceled;
            _reloadAction.started += OnReload;
        }

        private void OnDisable()
        {
            // при выгрузке сцены PlayerInput может быть уничтожен раньше нас,
            // поэтому отписываемся через сохранённый action, а не через PlayerInput
            if (_fireAction != null)
            {
                _fireAction.performed -= OnFirePerformed;
                _fireAction.canceled -= OnFireCanceled;
                _fireAction = null;
            }

            if (_reloadAction != null)
            {
                _reloadAction.started -= OnReload;
                _reloadAction = null;
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
