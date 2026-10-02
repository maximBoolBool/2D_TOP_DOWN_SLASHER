using Assets.Scripts._Project.Code.Slasher.Core.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Interactables
{
    /// <summary>
    /// Зона выхода с уровня. Когда игрок в зоне и ворота открыты — уровень пройден
    /// (LevelCompletedMessage в SignalBus). Срабатывает один раз.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class LevelExitZone : MonoBehaviour
    {
        [SerializeField, Tooltip("Ворота, которые нужно открыть. Пусто — выход открыт всегда")]
        private LevelGate _gate;

        private SignalBus _signalBus;
        private bool _isCompleted;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        // Stay, а не Enter: игрок может стоять в зоне ещё до того, как открыл ворота
        private void OnTriggerStay2D(Collider2D other)
        {
            if (_isCompleted || !other.CompareTag(TagConstants.PLAYER))
            {
                return;
            }

            if (_gate != null && !_gate.IsOpened)
            {
                return;
            }

            _isCompleted = true;
            _signalBus.Fire(new LevelCompletedMessage());
        }
    }
}
