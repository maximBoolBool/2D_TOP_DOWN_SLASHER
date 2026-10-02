using Assets.Scripts._Project.Code.Slasher.Game.Services;
using Assets.Scripts._Project.Code.Slasher.Main.Services;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Main.Bootstraps
{
    public class GameBootstrap : IInitializable
    {
        [Inject]
        private readonly IUserHealthBarService _userHealthBarService;

        [Inject]
        private readonly IWeaponUILoadService _weaponUILoadService;

        [Inject]
        private readonly IGameUiScreenService _gameUiScreenService;

        [Inject]
        private readonly ILevelProgressService _levelProgressService;

        public void Initialize()
        {
            _levelProgressService.LoadSelectedLevel();

            _userHealthBarService.Subscribe();
            _weaponUILoadService.Subscribe();
            _gameUiScreenService.Subscribe();
            _levelProgressService.Subscribe();
        }
    }
}
