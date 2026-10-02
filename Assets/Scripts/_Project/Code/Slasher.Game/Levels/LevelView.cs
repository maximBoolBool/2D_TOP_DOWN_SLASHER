using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Game.Levels
{
    /// <summary>
    /// Корень префаба уровня. Всё, что относится к конкретному уровню
    /// (враги, ворота, оружие, выход, точка старта игрока), лежит внутри префаба уровня,
    /// а не в игровой сцене — поэтому на каждом уровне всё стоит на своих местах.
    /// </summary>
    public class LevelView : MonoBehaviour
    {
        [SerializeField, Tooltip("Где появляется игрок при загрузке уровня")]
        private Transform _playerSpawnPoint;

        public Transform PlayerSpawnPoint => _playerSpawnPoint;

        private void OnDrawGizmos()
        {
            if (_playerSpawnPoint == null)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_playerSpawnPoint.position, 0.5f);
            Gizmos.DrawLine(_playerSpawnPoint.position, _playerSpawnPoint.position + Vector3.up);
        }
    }
}
