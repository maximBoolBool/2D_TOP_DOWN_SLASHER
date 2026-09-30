using Assets.Scripts._Project.Code.Slasher.Game.Services;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Bootstraps
{
    public class GameBootstrap : IInitializable
    {
        [Inject]
        private readonly IUserHealthBarService _userHealthBarService;

        [Inject]
        private readonly IWeaponUILoadService _weaponUILoadService;

        public void Initialize()
        {
            _userHealthBarService.Subscribe();
            _weaponUILoadService.Subscribe();
        }
    }
}
