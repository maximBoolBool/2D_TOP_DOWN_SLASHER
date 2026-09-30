using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic
{
    /// <summary>
    /// Пересылает изменения здоровья юнита в SignalBus (HealthChangeMessage).
    /// Вешается только на юнит игрока.
    /// </summary>
    [RequireComponent(typeof(UnitCharecteristic))]
    public class PlayerHealthSignalSender : MonoBehaviour
    {
        private SignalBus _signalBus;
        private UnitCharecteristic _unit;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _unit = GetComponent<UnitCharecteristic>();
        }

        private void OnEnable()
        {
            _unit.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _unit.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int current, int max)
        {
            _signalBus.Fire(new HealthChangeMessage(current, max));
        }
    }
}
