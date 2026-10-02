using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Ui.MonoBehaviours
{
    /// <summary>
    /// Экран UI с анимацией появления/скрытия.
    /// Появление: экран проявляется и подъезжает сбоку, затем его кнопки (AnimatedButton) по очереди выпрыгивают.
    /// Скрытие: экран гаснет и уезжает в противоположную сторону.
    /// direction: 1 — переход «вперёд», -1 — «назад».
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UiScreen : MonoBehaviour
    {
        [SerializeField]
        private float _showDuration = 0.2f;

        [SerializeField]
        private float _hideDuration = 0.15f;

        [SerializeField, Tooltip("На сколько пикселей экран сдвигается при переходе")]
        private float _slideOffset = 32f;

        [SerializeField]
        private float _buttonAppearDuration = 0.2f;

        [SerializeField, Tooltip("Задержка между появлением соседних кнопок")]
        private float _buttonAppearStep = 0.05f;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private AnimatedButton[] _buttons;
        private Vector2 _homePosition;
        private bool _isInitialized;

        public Tween Show(int direction)
        {
            Initialize();
            transform.DOKill();

            SetInteractable(false);
            _canvasGroup.alpha = 0f;
            _rectTransform.anchoredPosition = _homePosition + new Vector2(direction * _slideOffset, 0f);
            foreach (var button in _buttons)
            {
                button.PrepareAppear();
            }

            gameObject.SetActive(true);

            var sequence = DOTween.Sequence()
                .Append(Fade(1f, _showDuration, Ease.OutCubic))
                .Join(SlideTo(_homePosition, _showDuration, Ease.OutCubic));

            float buttonsStart = _showDuration * 0.5f;
            for (int i = 0; i < _buttons.Length; i++)
            {
                sequence.Insert(buttonsStart + i * _buttonAppearStep, _buttons[i].Appear(_buttonAppearDuration));
            }

            return sequence
                .OnComplete(() => SetInteractable(true))
                .SetUpdate(true)
                .SetLink(gameObject)
                .SetTarget(transform);
        }

        public Tween Hide(int direction)
        {
            Initialize();
            transform.DOKill();
            SetInteractable(false);

            var target = _homePosition - new Vector2(direction * _slideOffset, 0f);
            return DOTween.Sequence()
                .Append(Fade(0f, _hideDuration, Ease.InQuad))
                .Join(SlideTo(target, _hideDuration, Ease.InQuad))
                .OnComplete(HideImmediate)
                .SetUpdate(true)
                .SetLink(gameObject)
                .SetTarget(transform);
        }

        public void HideImmediate()
        {
            Initialize();
            SetInteractable(false);
            gameObject.SetActive(false);
            _canvasGroup.alpha = 1f;
            _rectTransform.anchoredPosition = _homePosition;
        }

        // Awake у выключенного объекта не вызывается, поэтому инициализируемся лениво при первом обращении
        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = (RectTransform)transform;
            _buttons = GetComponentsInChildren<AnimatedButton>(true);
            _homePosition = _rectTransform.anchoredPosition;
            _isInitialized = true;
        }

        private void SetInteractable(bool value)
        {
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }

        private Tween Fade(float alpha, float duration, Ease ease)
        {
            return DOTween.To(() => _canvasGroup.alpha, a => _canvasGroup.alpha = a, alpha, duration)
                .SetEase(ease);
        }

        // позиция округляется до целых, чтобы пиксель-арт не дрожал
        private Tween SlideTo(Vector2 position, float duration, Ease ease)
        {
            return DOTween.To(
                    () => _rectTransform.anchoredPosition,
                    p => _rectTransform.anchoredPosition = new Vector2(Mathf.Round(p.x), Mathf.Round(p.y)),
                    position,
                    duration)
                .SetEase(ease);
        }
    }
}
