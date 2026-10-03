using Assets.Scripts._Project.Code.Slasher.Ui.Constants;
using Assets.Scripts._Project.Code.Slasher.Ui.MonoBehaviours;
using Assets.Scripts._Project.Code.Slasher.Ui.Services;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Main.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private UiScreen _mainMenuScreen;

        [SerializeField]
        private UiScreen _levelPeakScreen;

        public override void InstallBindings()
        {
            Container.Bind<UiScreen>()
                .WithId(UiInjectConstants.MAIN_MENU_SCREEN_ID)
                .FromInstance(_mainMenuScreen);

            Container.Bind<UiScreen>()
                .WithId(UiInjectConstants.LEVEL_PEAK_SCREEN_ID)
                .FromInstance(_levelPeakScreen);

            Container.BindInterfacesAndSelfTo<MainMenuUiService>().AsSingle();
        }
    }
}
