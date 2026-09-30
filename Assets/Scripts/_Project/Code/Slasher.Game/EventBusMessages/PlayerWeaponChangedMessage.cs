using Assets.Scripts._Project.Code.Slasher.Game.Weapons;

namespace Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages
{
    public readonly struct PlayerWeaponChangedMessage
    {
        public RangeWeapon Weapon { get; }

        public PlayerWeaponChangedMessage(RangeWeapon weapon)
        {
            Weapon = weapon;
        }
    }
}
