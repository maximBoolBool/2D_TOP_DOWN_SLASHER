using Assets.Scripts._Project.Code.Slasher.Core.Enums;
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

        public void GoMainMenu()
        {
            _sceneLoadService.SwitchSceneAsync(GameGlobalStateType.Menu).Forget();
        }
    }
}
