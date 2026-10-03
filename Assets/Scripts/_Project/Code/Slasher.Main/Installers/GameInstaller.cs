using System.Collections.Generic;
using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers;
using Assets.Scripts._Project.Code.Slasher.Game.Services;
using Assets.Scripts._Project.Code.Slasher.Main.Bootstraps;
using Assets.Scripts._Project.Code.Slasher.Main.Services;
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

        [SerializeField, Tooltip("Префабы уровней. Имя префаба должно совпадать с тем, что передаёт кнопка выбора уровня")]
        private List<GameObject> _levelPrefabs = new();

        [SerializeField, Tooltip("Куда создаётся уровень (_Environment)")]
        private Transform _levelRoot;

        [SerializeField, Tooltip("Игрок: при загрузке уровня ставится на его PlayerSpawnPoint")]
        private Transform _playerUnit;

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

            Container.Bind<GameObject>()
                .WithId(GameObjectInjectConstants.WIN_SCREEN_GO_ID)
                .FromInstance(_winScreen);

            Container.Bind<GameObject>()
                .WithId(GameObjectInjectConstants.DEFEAT_SCREEN_GO_ID)
                .FromInstance(_defeatScreen);

            Container.Bind<IGameUiScreenService>().To<GameUiScreenService>().AsSingle();

            BindLevels();

            BindUserControllers();

            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();
        }

        private void BindLevels()
        {
            Container.Bind<List<GameObject>>()
                .WithId(GameObjectInjectConstants.LEVEL_PREFAB_IDS)
                .FromInstance(_levelPrefabs);

            Container.Bind<Transform>()
                .WithId(GameObjectInjectConstants.LEVEL_ROOT_ID)
                .FromInstance(_levelRoot);

            Container.Bind<Transform>()
                .WithId(GameObjectInjectConstants.PLAYER_UNIT_ID)
                .FromInstance(_playerUnit);

            Container.Bind<ILevelLoadService>().To<LevelLoadService>().AsSingle();
            Container.Bind<ILevelProgressService>().To<LevelProgressService>().AsSingle();
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
            Container.DeclareSignal<SetGameUiScreenMessage>();
            Container.DeclareSignal<LevelCompletedMessage>();
        }
    }
}
