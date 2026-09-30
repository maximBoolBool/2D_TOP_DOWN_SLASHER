using Assets.Scripts._Project.Code.Slasher.Core.Models;
using Assets.Scripts._Project.Code.Slasher.Core.Services;
using Assets.Scripts._Project.Code.Slasher.Core.Ui;
using UnityEngine;
using Assets.Scripts._Project.Code.Slasher.Main.Bootstrap;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Main.Installers
{
    public class RootInstaller : MonoInstaller
    {
        [SerializeField]
        private LoadingScreen _loadingScreen;

        public override void InstallBindings()
        {
            Container.Bind<ILoadingScreen>().FromInstance(_loadingScreen).AsSingle();
            Container.Bind<IGameGlobalStateManager>().To<GameGlobalState>().AsSingle();
            Container.Bind<ISceneLoadService>().To<SceneLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AppBootstrap>().AsSingle().NonLazy();
        }
    }
}
