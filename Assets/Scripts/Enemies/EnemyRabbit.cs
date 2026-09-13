using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyRabbit : BaseEnemy
    {
        [SerializeField] private float _aimUpdateInterval = 0.5f;
        private float _aimTimer;
        private float _weaponAimAngle;
        private Weapon _weapon;

        private void Awake()
        {
            BaseAwake();
            _weapon = GetComponentInChildren<Weapon>();
        }

        private void FixedUpdate()
        {
            Execute();
        }

        public override void Execute()
        {
            if (!IsExecutionAvailable())
            {
                return;
            }

            if (_aimUpdateInterval == 0)
            {
                UpdateAim();
            }
            else
            {
                _aimTimer += Time.fixedDeltaTime;
                if (_aimTimer >= _aimUpdateInterval)
                {
                    UpdateAim();
                    _aimTimer = 0f;
                }
            }

            _weapon.transform.rotation = Quaternion.Euler(0f, 0f, _weaponAimAngle);
            _weapon.Execute();
        }

        private void UpdateAim()
        {
            if (player == null) return;

            Vector3 aimDirection = (player.transform.position - transform.position).normalized;
            _weaponAimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        }
    }
}
