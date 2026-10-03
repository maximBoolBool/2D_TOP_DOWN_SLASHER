using Assets.Scripts._Project.Code.Slasher.Ui.Constants;
using Assets.Scripts._Project.Code.Slasher.Ui.Enums;
using Assets.Scripts._Project.Code.Slasher.Ui.MonoBehaviours;
using DG.Tweening;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Ui.Services
{
    public interface IMainMenuUiService
    {
        void SetActiveScreen(MainMenuScreenType screenType);
    }

    /// <summary>
    /// Переключает экраны главного меню: прячет текущий и показывает новый.
    /// Как именно экран анимируется, решает сам UiScreen.
    /// </summary>
    public class MainMenuUiService : IMainMenuUiService, IInitializable
    {
        private readonly UiScreen _mainMenuScreen;
        private readonly UiScreen _levelPeakScreen;

        private MainMenuScreenType? _currentType;
        private Tween _hideTween;

        public MainMenuUiService(
            [Inject(Id = UiInjectConstants.MAIN_MENU_SCREEN_ID)] UiScreen mainMenuScreen,
            [Inject(Id = UiInjectConstants.LEVEL_PEAK_SCREEN_ID)] UiScreen levelPeakScreen)
        {
            _mainMenuScreen = mainMenuScreen;
            _levelPeakScreen = levelPeakScreen;
        }

        public void Initialize()
        {
            _mainMenuScreen.HideImmediate();
            _levelPeakScreen.HideImmediate();

            SetActiveScreen(MainMenuScreenType.MainMenu);
        }

        public void SetActiveScreen(MainMenuScreenType screenType)
        {
            if (_currentType == screenType)
            {
                return;
            }

            // предыдущий экран мог ещё не доуехать — доводим его скрытие до конца
            _hideTween?.Kill(complete: true);

            int direction = _currentType == null || screenType > _currentType ? 1 : -1;

            if (_currentType.HasValue)
            {
                _hideTween = GetScreen(_currentType.Value).Hide(direction);
            }

            var showTween = GetScreen(screenType).Show(direction);
            if (_hideTween != null && _hideTween.IsActive())
            {
                // новый экран начинает появляться, когда старый уже ушёл
                showTween.SetDelay(_hideTween.Duration());
            }

            _currentType = screenType;
        }

        private UiScreen GetScreen(MainMenuScreenType screenType)
        {
            return screenType switch
            {
                MainMenuScreenType.MainMenu => _mainMenuScreen,
                MainMenuScreenType.LevelPeak => _levelPeakScreen,
                _ => throw new System.ArgumentOutOfRangeException(nameof(screenType), screenType, null),
            };
        }
    }
}
