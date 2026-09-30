using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Services
{
    public interface IUserHealthBarService : IEventBusConsumer { }

    public class UserHealthBarService : IUserHealthBarService
    {
        private readonly SignalBus _signalBus;
        private readonly GameObject _healthBarGO;
        private readonly RectTransform _fill;

        public UserHealthBarService(
            [Inject]  SignalBus signalBus,
            [Inject(Id = GameObjectInjectConstants.USER_HEALTH_BAR_GO)] GameObject healthBarGO
        )
        {
            _signalBus = signalBus;
            _healthBarGO = healthBarGO;
            _fill = _healthBarGO.transform.Find("FillArea").Find("Fill").GetComponent<RectTransform>();
        }

        public void Subscribe()
        {
            _signalBus.Subscribe<HealthChangeMessage>(SetHealthValue);
        }

        public void Unsubscribe()
        {
            _signalBus.Unsubscribe<HealthChangeMessage>(SetHealthValue);
        }

        private void SetHealthValue(HealthChangeMessage message)
        {
            int max = message.MaxHealth;
            int current = message.CurrentHealth;
            float ratio = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;

            _fill.anchorMax = new Vector2(ratio, 1f);
            _fill.gameObject.SetActive(ratio > 0f);
        }
        public void Dispose()
        {
            Unsubscribe();
        }
    }
}
