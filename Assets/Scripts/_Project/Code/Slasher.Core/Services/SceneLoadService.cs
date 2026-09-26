using Assets.Scripts._Project.Code.Slasher.Core.Constants;
using Assets.Scripts._Project.Code.Slasher.Core.Enums;
using Assets.Scripts._Project.Code.Slasher.Core.Models;
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

        public SceneLoadService(
            IGameGlobalStateManager gameGlobalStateManager,
            ZenjectSceneLoader zenjectSceneLoader)
        {
            _gameGlobalStateManager = gameGlobalStateManager;
            _zenjectSceneLoader = zenjectSceneLoader;
        }

        public async UniTask SwitchSceneAsync(GameGlobalStateType newSceneType)
        {
            string previousSceneName = _gameGlobalStateManager.CurrentState.GetSceneName();
            string newSceneName = newSceneType.GetSceneName();

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
