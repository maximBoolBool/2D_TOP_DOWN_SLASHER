using System.Collections.Generic;
using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.Enums;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Services
{
    /// <summary>
    /// Показывает игровые экраны (поражение / победа) по SetGameUiScreenMessage.
    /// </summary>
    public interface IGameUiScreenService : IEventBusConsumer { }

    public class GameUiScreenService : IGameUiScreenService
    {
        private readonly SignalBus _signalBus;
        private readonly Dictionary<UiScreenType, GameObject> _screens;

        public GameUiScreenService(
            SignalBus signalBus,
            [Inject(Id = GameObjectInjectConstants.DEFEAT_SCREEN_GO_ID)] GameObject defeatScreen,
            [Inject(Id = GameObjectInjectConstants.WIN_SCREEN_GO_ID)] GameObject winScreen)
        {
            _signalBus = signalBus;
            _screens = new Dictionary<UiScreenType, GameObject>
            {
                { UiScreenType.DefeatScreen, defeatScreen },
                { UiScreenType.WinScreen, winScreen },
            };
        }

        public void Subscribe()
        {
            HideAll();
            _signalBus.Subscribe<SetGameUiScreenMessage>(OnSetScreen);
        }

        public void Unsubscribe()
        {
            _signalBus.TryUnsubscribe<SetGameUiScreenMessage>(OnSetScreen);
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void OnSetScreen(SetGameUiScreenMessage message)
        {
            if (!_screens.TryGetValue(message.ScreenType, out var screen) || screen == null)
            {
                Debug.LogWarning($"{nameof(GameUiScreenService)}: screen {message.ScreenType} is not assigned in GameInstaller");
                return;
            }

            HideAll();
            screen.SetActive(true);
        }

        private void HideAll()
        {
            foreach (var screen in _screens.Values)
            {
                if (screen != null)
                {
                    screen.SetActive(false);
                }
            }
        }
    }
}
