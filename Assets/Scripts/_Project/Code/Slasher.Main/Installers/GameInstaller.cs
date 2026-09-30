using Assets.Scripts._Project.Code.Slasher.Game.Bootstraps;
using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using Assets.Scripts._Project.Code.Slasher.Game.Services;
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

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            InjectMessages();

            Container.Bind<GameObject>()
                .WithId(GameObjectInjectConstants.WEAPON_LOAD_GO)
                .FromInstance(_weaponLoadGO);

            Container.Bind<GameObject>()
                .WithId(GameObjectInjectConstants.USER_HEALTH_BAR_GO)
                .FromInstance(_userHealthBarGO);

            Container.Bind<IUserHealthBarService>().To<UserHealthBarService>().AsSingle();
            Container.Bind<IWeaponUILoadService>().To<WeaponUILoadService>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();
        }

        private void InjectMessages()
        {
            Container.DeclareSignal<HealthChangeMessage>();
        }
    }
}
