using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts._Project.Code.Slasher.Ui.MonoBehaviours
{
    /// <summary>
    /// Анимация кнопки: подрастает при наведении, «вжимается» при нажатии,
    /// умеет «выпрыгивать» при появлении экрана (вызывает UiScreen).
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class AnimatedButton : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private float _hoverScale = 1.1f;

        [SerializeField]
        private float _pressedScale = 0.9f;

        [SerializeField]
        private float _scaleDuration = 0.08f;

        private Button _button;
        private bool _isHovered;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnDisable()
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            _isHovered = false;
        }

        /// <summary>Мгновенно прячет кнопку (scale 0) перед анимацией появления.</summary>
        public void PrepareAppear()
        {
            transform.DOKill();
            transform.localScale = Vector3.zero;
        }

        /// <summary>Появление из нуля с небольшим перелётом.</summary>
        public Tween Appear(float duration)
        {
            return transform.DOScale(Vector3.one, duration)
                .From(Vector3.zero)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            ScaleTo(_hoverScale);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            ScaleTo(1f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ScaleTo(_pressedScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ScaleTo(_isHovered ? _hoverScale : 1f);
        }

        private void ScaleTo(float scale)
        {
            // во время перехода экрана кнопки неинтерактивны (CanvasGroup) — не мешаем анимации появления
            if (_button == null || !_button.IsInteractable())
            {
                return;
            }

            transform.DOKill();
            transform.DOScale(Vector3.one * scale, _scaleDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(gameObject);
        }
    }
}
