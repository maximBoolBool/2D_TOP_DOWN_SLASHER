using Assets.Scripts._Project.Code.Slasher.Core.Enums;
using Assets.Scripts._Project.Code.Slasher.Core.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Ui.MonoBehaviours
{
    public class MainMenuButtonBehaviour : MonoBehaviour
    {
        [Inject]
        private ISceneLoadService _sceneLoadService;

        public void StartGame()
        {
            _sceneLoadService.SwitchSceneAsync(GameGlobalStateType.Game).Forget();
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
