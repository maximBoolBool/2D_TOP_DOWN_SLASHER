using System.Collections;
using Assets.Scripts._Project.Code.Slasher.Game.Enums;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic
{
    /// <summary>
    /// Пересылает события юнита игрока в SignalBus:
    /// изменение здоровья -> HealthChangeMessage,
    /// смерть -> (ждём анимацию смерти) -> SetGameUiScreenMessage(DefeatScreen).
    /// Вешается только на юнит игрока.
    /// </summary>
    [RequireComponent(typeof(UnitCharecteristic))]
    public class PlayerHealthSignalSender : MonoBehaviour
    {
        private const int BaseLayerIndex = 0;

        [SerializeField, Tooltip("Пауза после окончания анимации смерти перед показом экрана поражения")]
        private float _defeatScreenDelay = 0.3f;

        [SerializeField, Tooltip("Страховка: максимум ожидания анимации смерти, сек")]
        private float _maxDeathAnimationWait = 3f;

        private SignalBus _signalBus;
        private UnitCharecteristic _unit;
        private Animator _animator;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _unit = GetComponent<UnitCharecteristic>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _unit.HealthChanged += OnHealthChanged;
            _unit.Died += OnDied;
        }

        private void OnDisable()
        {
            _unit.HealthChanged -= OnHealthChanged;
            _unit.Died -= OnDied;
        }

        private void OnHealthChanged(int current, int max)
        {
            _signalBus.Fire(new HealthChangeMessage(current, max));
        }

        private void OnDied()
        {
            StartCoroutine(ShowDefeatAfterDeathAnimation());
        }

        private IEnumerator ShowDefeatAfterDeathAnimation()
        {
            yield return WaitForDeathAnimation();

            if (_defeatScreenDelay > 0f)
            {
                yield return new WaitForSeconds(_defeatScreenDelay);
            }

            _signalBus.Fire(new SetGameUiScreenMessage(UiScreenType.DefeatScreen));
        }

        // DeadTrigger срабатывает в следующем апдейте аниматора:
        // ждём, пока закончится переход, и затем длину стейта смерти
        private IEnumerator WaitForDeathAnimation()
        {
            if (_animator == null)
            {
                yield break;
            }

            float elapsed = 0f;

            yield return null;
            elapsed += Time.deltaTime;

            while (_animator.IsInTransition(BaseLayerIndex) && elapsed < _maxDeathAnimationWait)
            {
                yield return null;
                elapsed += Time.deltaTime;
            }

            var state = _animator.GetCurrentAnimatorStateInfo(BaseLayerIndex);
            float remaining = state.length * Mathf.Max(0f, 1f - state.normalizedTime);
            float wait = Mathf.Min(remaining, Mathf.Max(0f, _maxDeathAnimationWait - elapsed));

            if (wait > 0f)
            {
                yield return new WaitForSeconds(wait);
            }
        }
    }
}
