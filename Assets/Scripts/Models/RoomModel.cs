using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class RoomModel
    {
        public List<EnemyWave> Waves { get; set; } = new List<EnemyWave>();
        
        public Dictionary<int, GameObject[]> SpawnCoordinates { get; set; } = new Dictionary<int, GameObject[]>();
    }
}
