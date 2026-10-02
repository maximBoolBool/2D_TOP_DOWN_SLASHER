using Assets.Scripts._Project.Code.Slasher.Game.Enums;

namespace Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages
{
    public class SetGameUiScreenMessage
    {
        public UiScreenType ScreenType { get; private set; }
    }
}
