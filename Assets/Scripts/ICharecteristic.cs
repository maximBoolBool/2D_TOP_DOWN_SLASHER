using Assets.Scripts.Enums;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public interface ICharecteristic
    {
        public Dictionary<CharecteristicType, int> BaseCharecteristics { get; set; }
        public Dictionary<CharecteristicType, int> ActualCharacteristics { get; set; }
    }
}
