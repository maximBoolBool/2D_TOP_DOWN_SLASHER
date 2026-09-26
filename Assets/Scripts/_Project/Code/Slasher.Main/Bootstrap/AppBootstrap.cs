using Assets.Scripts._Project.Code.Slasher.Core.Constants;
using Assets.Scripts._Project.Code.Slasher.Core.Enums;
using Assets.Scripts._Project.Code.Slasher.Core.Models;
using Assets.Scripts._Project.Code.Slasher.Core.Services;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Main.Bootstrap
{
    public class AppBootstrap : IInitializable
    {
        [Inject]
        private readonly ISceneLoadService _sceneLoadService;

        [Inject]
        private readonly IGameGlobalStateManager _gameGlobalStateManager;

        public void Initialize()
        {
            var alreadyOpenSceneType = FindAlreadyOpenSceneType();

            if (alreadyOpenSceneType.HasValue)
            {
                _gameGlobalStateManager.SetStatus(alreadyOpenSceneType.Value);
                return;
            }

            _sceneLoadService.SwitchScene(GameGlobalStateType.Menu);
        }

        private GameGlobalStateType? FindAlreadyOpenSceneType()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded || scene.name == SceneNamesConstants.ROOT)
                {
                    continue;
                }

                if (scene.name == SceneNamesConstants.MAIN_MENU)
                {
                    return GameGlobalStateType.Menu;
                }

                if (scene.name == SceneNamesConstants.GAME)
                {
                    return GameGlobalStateType.Game;
                }
            }

            return null;
        }
    }
}
