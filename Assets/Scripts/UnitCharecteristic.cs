using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnitCharecteristic : MonoBehaviour, ICharecteristic
    {
        [SerializeField]
        private int speed = 1;
        [SerializeField]
        private int health = 1;

        private UnitStatusType _unitStatusType;

        public void Awake()
        {
            BaseCharecteristics = new Dictionary<CharecteristicType, int>
            {
                { CharecteristicType.Speed, speed },
                { CharecteristicType.Health, health }
            };
            ActualCharacteristics = new Dictionary<CharecteristicType, int>(BaseCharecteristics);
            _unitStatusType = UnitStatusType.Alive;
        }

        public Dictionary<CharecteristicType, int> BaseCharecteristics { get; set; }
        public Dictionary<CharecteristicType, int> ActualCharacteristics { get; set; }

        public void SetStatus(UnitStatusType newUnitStatus)
        {
            _unitStatusType = newUnitStatus;
        }

        public void SetDamage(int damage)
        {
            if (!ActualCharacteristics.TryGetValue(CharecteristicType.Health, out int currentHealth))
            {
                Debug.LogWarning("Health characteristic not found.");
                return;
            }

            var resultHealthPoints = Mathf.Max(currentHealth - damage, 0);
            ActualCharacteristics[CharecteristicType.Health] = resultHealthPoints;

            if (resultHealthPoints == 0)
            {
                UnitAnimationHelper.SetDeadAnimation(GetComponent<Animator>());
            }
        }
    }
}
