using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, ICharecteristic
    {
        protected Rigidbody2D rb;

        public string Name { get; set; }
        public Dictionary<CharecteristicType, int> BaseCharecteristics { get; set; }
        public Dictionary<CharecteristicType, int> ActualCharacteristics { get; set; }
        public abstract void Execute();

        protected void BaseAwake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
}
