using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Core.Ui
{
    /// <summary>
    /// Экран загрузки в Root-сцене: чёрный фон и вращающийся патрон.
    /// Показывается SceneLoadService на время переключения сцен.
    /// </summary>
    public interface ILoadingScreen
    {
        bool IsVisible { get; }

        void Show();
        void Hide();
    }

    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField]
        private RectTransform _spinner;

        [SerializeField]
        private float _rotationSpeed = 270f;

        public bool IsVisible => gameObject.activeSelf;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_spinner == null)
            {
                return;
            }

            // unscaledDeltaTime — чтобы патрон крутился, даже если игра на паузе (timeScale = 0)
            _spinner.Rotate(0f, 0f, -_rotationSpeed * Time.unscaledDeltaTime);
        }
    }
}
