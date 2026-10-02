using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers;
using Assets.Scripts._Project.Code.Slasher.Game.Services;
using Assets.Scripts._Project.Code.Slasher.Main.Bootstraps;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Main.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _weaponLoadGO;

        [SerializeField]
        private GameObject _userHealthBarGO;

        [SerializeField]
        private GameObject _winScreen;

        [SerializeField]
        private GameObject _defeatScreen;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            InjectMessages();

            Container.Bind<GameObject>()
                .WithId(GameObjectInjectConstants.WEAPON_LOAD_GO_ID)
                .FromInstance(_weaponLoadGO);

            Container.Bind<GameObject>()
                .WithId(GameObjectInjectConstants.USER_HEALTH_BAR_GO_ID)
                .FromInstance(_userHealthBarGO);

            Container.Bind<IUserHealthBarService>().To<UserHealthBarService>().AsSingle();
            Container.Bind<IWeaponUILoadService>().To<WeaponUILoadService>().AsSingle();

            BindUserControllers();

            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();
        }

        private void BindUserControllers()
        {
            Container.Bind<PlayerInteractController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerShootController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerWeaponAimController>().FromComponentInHierarchy().AsSingle();
        }

        private void InjectMessages()
        {
            Container.DeclareSignal<HealthChangeMessage>();
            Container.DeclareSignal<PlayerWeaponChangedMessage>();
        }
    }
}
