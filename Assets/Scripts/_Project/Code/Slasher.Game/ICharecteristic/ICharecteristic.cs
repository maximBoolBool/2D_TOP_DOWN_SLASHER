using System.Collections.Generic;
using Assets.Scripts._Project.Code.Slasher.Game.Enums;

namespace Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic
{
    public interface ICharecteristic
    {
        public Dictionary<CharecteristicType, int> BaseCharecteristics { get; set; }
        public Dictionary<CharecteristicType, int> ActualCharacteristics { get; set; }
    }
}
