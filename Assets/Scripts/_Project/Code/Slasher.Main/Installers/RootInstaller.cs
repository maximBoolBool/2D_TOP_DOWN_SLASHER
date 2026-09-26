using Assets.Scripts._Project.Code.Slasher.Core.Models;
using Assets.Scripts._Project.Code.Slasher.Core.Services;
using Assets.Scripts._Project.Code.Slasher.Main.Bootstrap;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Main.Installers
{
    public class RootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameGlobalStateManager>().To<GameGlobalState>().AsSingle();
            Container.Bind<ISceneLoadService>().To<SceneLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AppBootstrap>().AsSingle().NonLazy();
        }
    }
}
