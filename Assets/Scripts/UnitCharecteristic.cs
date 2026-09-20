using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using System.Collections;
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

        public bool IsAlive => _unitStatusType == UnitStatusType.Alive;

        public void SetDamage(int damage)
        {
            if (!ActualCharacteristics.TryGetValue(CharecteristicType.Health, out int currentHealth))
            {
                return;
            }

            var resultHealthPoints = Mathf.Max(currentHealth - damage, 0);
            ActualCharacteristics[CharecteristicType.Health] = resultHealthPoints;

            var animator = GetComponent<Animator>();
            if (resultHealthPoints == 0)
            {
                UnitAnimationHelper.SetDeadAnimation(animator);
                SetStatus(UnitStatusType.Dead);
            }
            else
            {
                UnitAnimationHelper.SetDamagedAnimation(animator);
                StartCoroutine(ResetLayerWeight(animator, animator.GetLayerIndex(UnitAnimationHelper.HitLayer), 0.25f));
            }
        }

        private IEnumerator ResetLayerWeight(Animator animator, int layerIndex, float delay)
        {
            yield return new WaitForSeconds(delay);
            animator.SetLayerWeight(layerIndex, 0f);
        }
    }
}
