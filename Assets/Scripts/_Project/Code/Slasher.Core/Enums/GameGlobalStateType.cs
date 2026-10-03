using Assets.Scripts._Project.Code.Slasher.Core.Constants;
using System;

namespace Assets.Scripts._Project.Code.Slasher.Core.Enums
{
    public enum GameGlobalStateType
    {
        Menu = 0,
        Game = 1
    }

    public static class GameStateTypeExtensions
    {
        public static string GetSceneName(this GameGlobalStateType gameStateType)
        {
            return gameStateType switch
            {
                GameGlobalStateType.Menu => SceneNamesConstants.MAIN_MENU,
                GameGlobalStateType.Game => SceneNamesConstants.GAME,
                _ => throw new ArgumentOutOfRangeException(nameof(gameStateType), gameStateType, null),
            };
        }
    }
}
