using System.Collections.Generic;
using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using Assets.Scripts._Project.Code.Slasher.Game.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Services
{
    /// <summary>
    /// UI патронов игрока. Узнаёт о смене оружия через PlayerWeaponChangedMessage,
    /// а патроны/перезарядку слушает у самого текущего оружия.
    /// </summary>
    public interface IWeaponUILoadService : IEventBusConsumer { }

    public class WeaponUILoadService : IWeaponUILoadService
    {
        private const string AmmoQueueName = "AmmoQueue";
        private const string AmmoCounterTextName = "AmmoCounterText";
        private const string ReloadingText = "RELOAD";
        private const string InfinitySymbol = "INF";
        private const float IconSize = 16f;
        private const float IconSpacing = 1f;
        private const float IconRotation = 90f;
        private const int MaxVisibleRounds = 10;

        private static readonly Color LoadedColor = Color.white;
        private static readonly Color SpentColor = new(0.3f, 0.3f, 0.3f, 1f);

        private readonly SignalBus _signalBus;
        private readonly GameObject _weaponLoadGO;

        private readonly List<Image> _icons = new();
        private RectTransform _ammoQueue;
        private TextMeshProUGUI _counterText;
        private RangeWeapon _weapon;

        public WeaponUILoadService(
            SignalBus signalBus,
            [Inject(Id = GameObjectInjectConstants.WEAPON_LOAD_GO_ID)] GameObject weaponLoadGO)
        {
            _signalBus = signalBus;
            _weaponLoadGO = weaponLoadGO;
        }

        public void Subscribe()
        {
            _signalBus.Subscribe<PlayerWeaponChangedMessage>(OnPlayerWeaponChanged);
        }

        public void Unsubscribe()
        {
            _signalBus.Unsubscribe<PlayerWeaponChangedMessage>(OnPlayerWeaponChanged);
            DetachWeapon();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void OnPlayerWeaponChanged(PlayerWeaponChangedMessage message)
        {
            SetWeapon(message.Weapon);
        }

        private void SetWeapon(RangeWeapon weapon)
        {
            DetachWeapon();

            _weapon = weapon;

            if (_weapon != null)
            {
                _weapon.AmmoChanged += Refresh;
                _weapon.ReloadStarted += OnReloadStarted;
            }

            Refresh();
        }

        // отписываемся от патронов текущего оружия, UI не трогаем
        // (при выгрузке сцены он может быть уже уничтожен)
        private void DetachWeapon()
        {
            if (_weapon != null)
            {
                _weapon.AmmoChanged -= Refresh;
                _weapon.ReloadStarted -= OnReloadStarted;
            }

            _weapon = null;
        }

        private void OnReloadStarted(float duration)
        {
            if (!TryInitViews())
            {
                return;
            }

            _counterText.text = ReloadingText;
        }

        private void Refresh()
        {
            if (!TryInitViews())
            {
                return;
            }

            _weaponLoadGO.SetActive(_weapon != null);
            if (_weapon == null)
            {
                return;
            }

            int visibleSlots = Mathf.Clamp(_weapon.MagazineRounds, 0, MaxVisibleRounds);
            int visibleLoaded = Mathf.Min(_weapon.AmmoInMagazine, visibleSlots);
            EnsureIconCount(visibleSlots);

            float iconWidth = GetIconWidth(_weapon.AmmoIcon);
            float step = visibleSlots > 0
                ? Mathf.Floor(Mathf.Min(iconWidth + IconSpacing, _ammoQueue.rect.height / visibleSlots))
                : 0f;

            for (int i = 0; i < _icons.Count; i++)
            {
                var icon = _icons[i];
                bool isVisible = i < visibleSlots;
                icon.gameObject.SetActive(isVisible);

                if (!isVisible)
                {
                    continue;
                }

                icon.sprite = _weapon.AmmoIcon;
                icon.color = i < visibleLoaded ? LoadedColor : SpentColor;
                icon.rectTransform.sizeDelta = new Vector2(iconWidth, IconSize);
                icon.rectTransform.anchoredPosition = new Vector2(0f, i * step + iconWidth / 2f);
            }

            string reserve = _weapon.IsInfiniteAmmo ? InfinitySymbol : _weapon.AmmoInReserve.ToString();
            _counterText.text = $"{_weapon.AmmoInMagazine}-{reserve}";
        }

        // ширина иконки по пропорциям спрайта
        private static float GetIconWidth(Sprite sprite)
        {
            if (sprite == null || sprite.rect.height <= 0f)
            {
                return IconSize;
            }

            return Mathf.Round(IconSize * sprite.rect.width / sprite.rect.height);
        }

        private void EnsureIconCount(int count)
        {
            while (_icons.Count < count)
            {
                var go = new GameObject("AmmoIcon", typeof(RectTransform), typeof(Image));
                go.transform.SetParent(_ammoQueue, false);

                var rect = (RectTransform)go.transform;
                rect.anchorMin = new Vector2(0.5f, 0f);
                rect.anchorMax = new Vector2(0.5f, 0f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                // кладём патрон на бок, пулей влево
                rect.localRotation = Quaternion.Euler(0f, 0f, IconRotation);

                var image = go.GetComponent<Image>();
                image.preserveAspect = true;
                image.raycastTarget = false;

                _icons.Add(image);
            }
        }

        private bool TryInitViews()
        {
            if (_ammoQueue != null && _counterText != null)
            {
                return true;
            }

            if (_weaponLoadGO == null)
            {
                Debug.LogWarning($"{nameof(WeaponUILoadService)}: weapon load view is not assigned in GameInstaller");
                return false;
            }

            _ammoQueue = _weaponLoadGO.transform.Find(AmmoQueueName) as RectTransform;
            _counterText = _weaponLoadGO.transform.Find(AmmoCounterTextName)?.GetComponent<TextMeshProUGUI>();

            if (_ammoQueue == null || _counterText == null)
            {
                Debug.LogWarning($"{nameof(WeaponUILoadService)}: {AmmoQueueName} or {AmmoCounterTextName} not found under {_weaponLoadGO.name}", _weaponLoadGO);
                return false;
            }

            return true;
        }
    }
}
