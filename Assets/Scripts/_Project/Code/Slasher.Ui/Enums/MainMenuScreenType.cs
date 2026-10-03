namespace Assets.Scripts._Project.Code.Slasher.Ui.Enums
{
    /// <summary>
    /// Экраны главного меню. Порядок важен: экран с большим значением считается «дальше»
    /// (при переходе к нему анимация идёт вперёд, к меньшему — назад).
    /// </summary>
    public enum MainMenuScreenType
    {
        MainMenu = 0,
        LevelPeak = 1,
    }
}
