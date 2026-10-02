using Assets.Scripts._Project.Code.Slasher.Core.Enums;
using Assets.Scripts._Project.Code.Slasher.Core.Models;
using Assets.Scripts._Project.Code.Slasher.Core.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Ui.MonoBehaviours
{
    public class GameButtonsBehaviour : MonoBehaviour
    {
        [Inject]
        private ISceneLoadService _sceneLoadService;

        [Inject]
        private IGameGlobalStateManager _gameGlobalStateManager;

        public void GoMainMenu()
        {
            _sceneLoadService.SwitchSceneAsync(GameGlobalStateType.Menu).Forget();
        }

        /// <summary>Заново с первого уровня: игровая сцена перезагружается целиком (здоровье, оружие, враги — всё с нуля).</summary>
        public void RestartGame()
        {
            _gameGlobalStateManager.SetCurrentLevelName(null);
            _sceneLoadService.SwitchSceneAsync(GameGlobalStateType.Game).Forget();
        }
    }
}
