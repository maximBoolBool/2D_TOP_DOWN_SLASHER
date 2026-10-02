using System;
using Assets.Scripts._Project.Code.Slasher.Core.Models;
using Assets.Scripts._Project.Code.Slasher.Core.Ui;
using Assets.Scripts._Project.Code.Slasher.Game;
using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.Enums;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using Assets.Scripts._Project.Code.Slasher.Game.Levels;
using Assets.Scripts._Project.Code.Slasher.Game.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Assets.Scripts._Project.Code.Slasher.Main.Services
{
    /// <summary>
    /// Прохождение уровней внутри игровой сцены:
    /// загрузка выбранного в меню уровня и переход на следующий по LevelCompletedMessage.
    /// После последнего уровня показывает экран победы.
    /// </summary>
    public interface ILevelProgressService : IEventBusConsumer
    {
        void LoadSelectedLevel();
    }

    public class LevelProgressService : ILevelProgressService
    {
        private const int TransitionDelayMs = 300;

        private readonly SignalBus _signalBus;
        private readonly ILevelLoadService _levelLoadService;
        private readonly IGameGlobalStateManager _gameGlobalStateManager;
        private readonly ILoadingScreen _loadingScreen;
        private readonly Transform _playerUnit;

        private bool _isTransitioning;

        public LevelProgressService(
            SignalBus signalBus,
            ILevelLoadService levelLoadService,
            IGameGlobalStateManager gameGlobalStateManager,
            ILoadingScreen loadingScreen,
            [Inject(Id = GameObjectInjectConstants.PLAYER_UNIT_ID)] Transform playerUnit)
        {
            _signalBus = signalBus;
            _levelLoadService = levelLoadService;
            _gameGlobalStateManager = gameGlobalStateManager;
            _loadingScreen = loadingScreen;
            _playerUnit = playerUnit;
        }

        public void Subscribe()
        {
            _signalBus.Subscribe<LevelCompletedMessage>(OnLevelCompleted);
        }

        public void Unsubscribe()
        {
            _signalBus.TryUnsubscribe<LevelCompletedMessage>(OnLevelCompleted);
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        // уровень выбирается в меню (MainMenuButtonBehaviour.StartGame -> GameCurrentLevelName)
        public void LoadSelectedLevel()
        {
            string levelName = _gameGlobalStateManager.GameCurrentLevelName;

            LevelView level;
            if (string.IsNullOrEmpty(levelName))
            {
                // уровень не выбран (Restart или сцену запустили напрямую в редакторе) — начинаем с первого
                level = _levelLoadService.LoadLevel(0);
            }
            else
            {
                level = _levelLoadService.LoadLevel(levelName);
            }

            PlacePlayer(level);
        }

        private void OnLevelCompleted()
        {
            if (_isTransitioning)
            {
                return;
            }

            int nextIndex = _levelLoadService.CurrentLevelIndex + 1;
            if (nextIndex >= _levelLoadService.LevelsCount)
            {
                // последний уровень пройден
                _isTransitioning = true;
                _signalBus.Fire(new SetGameUiScreenMessage(UiScreenType.WinScreen));
                return;
            }

            TransitionToLevelAsync(nextIndex).Forget();
        }

        private async UniTaskVoid TransitionToLevelAsync(int levelIndex)
        {
            _isTransitioning = true;
            _loadingScreen.Show();

            try
            {
                await UniTask.Delay(TransitionDelayMs, ignoreTimeScale: true);

                DestroyFlyingBullets();
                var level = _levelLoadService.LoadLevel(levelIndex);
                if (level != null)
                {
                    _gameGlobalStateManager.SetCurrentLevelName(level.name);
                }

                PlacePlayer(level);

                // кадр на то, чтобы старый уровень успел удалиться, а новый — проинициализироваться
                await UniTask.Yield();
            }
            finally
            {
                _loadingScreen.Hide();
                _isTransitioning = false;
            }
        }

        // пули живут в сцене, а не в уровне — иначе долетят до нового уровня
        private static void DestroyFlyingBullets()
        {
            foreach (var bullet in Object.FindObjectsByType<Bullet>(FindObjectsSortMode.None))
            {
                Object.Destroy(bullet.gameObject);
            }
        }

        private void PlacePlayer(LevelView level)
        {
            if (level == null || level.PlayerSpawnPoint == null)
            {
                Debug.LogWarning($"{nameof(LevelProgressService)}: level has no PlayerSpawnPoint, player stays where it is");
                return;
            }

            Vector3 spawnPosition = level.PlayerSpawnPoint.position;
            _playerUnit.position = spawnPosition;

            // Rigidbody2D тоже переносим, иначе физика на первом кадре вернёт старую позицию
            if (_playerUnit.TryGetComponent(out Rigidbody2D body))
            {
                body.position = spawnPosition;
                body.linearVelocity = Vector2.zero;
            }
        }
    }
}
