using Assets.Scripts._Project.Code.Slasher.Core.Models;
using Assets.Scripts._Project.Code.Slasher.Core.Services;
using Assets.Scripts._Project.Code.Slasher.Ui.Enums;
using Assets.Scripts._Project.Code.Slasher.Ui.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Ui.MonoBehaviours
{
    public class MainMenuButtonBehaviour : MonoBehaviour
    {
        [Inject]
        private IMainMenuUiService _mainMenuUiService;

        [Inject]
        private ISceneLoadService _sceneLoadService;

        [Inject]
        private IGameGlobalStateManager _gameGlobalStateManager;

        public void OpenLevelPeakScreen()
        {
            _mainMenuUiService.SetActiveScreen(MainMenuScreenType.LevelPeak);
        }

        public void OpenMainMenuScreen()
        {
            _mainMenuUiService.SetActiveScreen(MainMenuScreenType.MainMenu);
        }

        public void StartGame(string levelName)
        {
            _gameGlobalStateManager.SetCurrentLevelName(levelName);
            _sceneLoadService.SwitchSceneAsync(Core.Enums.GameGlobalStateType.Game).Forget();
        }

        public void OnOptionClick()
        {
            Debug.Log("Option button clicked");
        }

        public void OnExitClick()
        {
            Application.Quit();
        }
    }
}
