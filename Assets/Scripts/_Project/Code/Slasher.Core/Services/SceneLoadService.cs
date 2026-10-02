using Assets.Scripts._Project.Code.Slasher.Core.Constants;
using Assets.Scripts._Project.Code.Slasher.Core.Enums;
using Assets.Scripts._Project.Code.Slasher.Core.Models;
using Assets.Scripts._Project.Code.Slasher.Core.Ui;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Core.Services
{
    public interface ISceneLoadService
    {
        UniTask SwitchSceneAsync(GameGlobalStateType newSceneType);
        UniTask UnloadSceneAsync(string sceneName);
        void SwitchScene(GameGlobalStateType newSceneType);
    }

    public class SceneLoadService : ISceneLoadService
    {
        private readonly IGameGlobalStateManager _gameGlobalStateManager;
        private readonly ZenjectSceneLoader _zenjectSceneLoader;
        private readonly ILoadingScreen _loadingScreen;

        public SceneLoadService(
            IGameGlobalStateManager gameGlobalStateManager,
            ZenjectSceneLoader zenjectSceneLoader,
            ILoadingScreen loadingScreen)
        {
            _gameGlobalStateManager = gameGlobalStateManager;
            _zenjectSceneLoader = zenjectSceneLoader;
            _loadingScreen = loadingScreen;
        }

        public async UniTask SwitchSceneAsync(GameGlobalStateType newSceneType)
        {
            _loadingScreen.Show();

            try
            {
                string previousSceneName = _gameGlobalStateManager.CurrentState.GetSceneName();
                string newSceneName = newSceneType.GetSceneName();

                // перезапуск той же сцены (например, Restart в игре): сначала выгружаем старую копию,
                // иначе Additive-загрузка создаст вторую сцену с тем же именем
                if (previousSceneName == newSceneName)
                {
                    await UnloadSceneAsync(previousSceneName);
                }

                await _zenjectSceneLoader
                    .LoadSceneAsync(newSceneName, LoadSceneMode.Additive)
                    .ToUniTask();

                var newlyLoadedScene = SceneManager.GetSceneByName(newSceneName);
                if (newlyLoadedScene.IsValid())
                {
                    SceneManager.SetActiveScene(newlyLoadedScene);
                    _gameGlobalStateManager.SetStatus(newSceneType);
                }

                if (previousSceneName != SceneNamesConstants.ROOT && previousSceneName != newSceneName)
                {
                    await UnloadSceneAsync(previousSceneName);
                }
            }
            finally
            {
                // прячем экран, даже если загрузка упала с ошибкой
                _loadingScreen.Hide();
            }
        }

        public void SwitchScene(GameGlobalStateType newSceneType)
        {
            string previousSceneName = _gameGlobalStateManager.CurrentState.GetSceneName();
            string newSceneName = newSceneType.GetSceneName();

            _zenjectSceneLoader
                .LoadScene(newSceneName, LoadSceneMode.Additive);

            var newlyLoadedScene = SceneManager.GetSceneByName(newSceneName);
            if (newlyLoadedScene.IsValid())
            {
                _gameGlobalStateManager.SetStatus(newSceneType);
            }
        }

        public async UniTask UnloadSceneAsync(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() && scene.isLoaded)
            {
                await SceneManager.UnloadSceneAsync(scene).ToUniTask();
                await Resources.UnloadUnusedAssets().ToUniTask();
            }
        }
    }
}
