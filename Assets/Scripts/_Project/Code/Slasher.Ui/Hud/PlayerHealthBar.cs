using Assets.Scripts._Project.Code.Slasher.Game.Enums;
using Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic;
using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Ui.Hud
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField]
        private UnitCharecteristic _target;

        [SerializeField]
        private RectTransform _fill;

        private void Start()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(PlayerHealthBar)}: target is not assigned", this);
                return;
            }

            _target.HealthChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (_target != null)
            {
                _target.HealthChanged -= Refresh;
            }
        }

        private void Refresh()
        {
            int max = _target.BaseCharecteristics[CharecteristicType.Health];
            int current = _target.ActualCharacteristics[CharecteristicType.Health];
            float ratio = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;

            _fill.anchorMax = new Vector2(ratio, 1f);
            _fill.gameObject.SetActive(ratio > 0f);
        }
    }
}
