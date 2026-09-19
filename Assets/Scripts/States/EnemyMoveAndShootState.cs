using Assets.Scripts.EnemyControllers;
using Assets.Scripts.Enums;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class EnemyMoveAndShootState : IState
    {
        private readonly BaseEnemy _enemy;
        private readonly EnemyMovementController _movement;
        private readonly EnemyAimController _aim;
        private readonly Weapon _weapon;
        private readonly float _windupTime;
        private readonly float _recoveryTime;
        private readonly float _attackRange;
        private readonly float _preferredDistance;

        private enum Phase { Windup, Shoot, Recovery }
        private Phase _currentPhase;
        private float _timer;

        public EnemyMoveAndShootState(
            BaseEnemy enemy,
            EnemyMovementController movement,
            EnemyAimController aim,
            Weapon weapon,
            float windupTime,
            float recoveryTime,
            float attackRange,
            float preferredDistance)
        {
            _enemy = enemy;
            _movement = movement;
            _aim = aim;
            _weapon = weapon;
            _windupTime = windupTime;
            _recoveryTime = recoveryTime;
            _attackRange = attackRange;
            _preferredDistance = preferredDistance;
        }

        public void Enter()
        {
            _timer = 0f;
            _currentPhase = Phase.Windup;
        }

        public void LogicUpdate()
        {
            if (_enemy.Player == null) return;

            _timer += Time.deltaTime;

            switch (_currentPhase)
            {
                case Phase.Windup:
                    _aim.AimAt(_enemy.Player.transform.position, _weapon.transform);
                    if (_timer >= _windupTime)
                    {
                        _currentPhase = Phase.Shoot;
                        _timer = 0f;
                    }
                    break;

                case Phase.Shoot:
                    _weapon.Execute();
                    _currentPhase = Phase.Recovery;
                    _timer = 0f;
                    break;

                case Phase.Recovery:
                    _aim.AimAt(_enemy.Player.transform.position, _weapon.transform);
                    if (_timer >= _recoveryTime)
                    {
                        _currentPhase = Phase.Windup;
                        _timer = 0f;
                    }
                    break;
            }

            // Если игрок убежал слишком далеко — возвращаемся в погоню
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.Player.transform.position);
            if (distance > _attackRange * 1.3f)
            {
                _enemy.StateMachine.ChangeState(_enemy.ChaseState);
            }
        }

        public void PhysicsUpdate()
        {
            if (_enemy.Player == null) return;

            float speed = _enemy.UnitCharacteristic.ActualCharacteristics[CharecteristicType.Speed];
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.Player.transform.position);

            Vector2 targetPosition;

            if (distance < _preferredDistance)
            {
                // Слишком близко — отходим (кайтинг), продолжая стрелять
                Vector2 awayDir = ((Vector2)_enemy.transform.position - (Vector2)_enemy.Player.transform.position).normalized;
                targetPosition = (Vector2)_enemy.transform.position + awayDir * 2f;
            }
            else
            {
                // Слишком далеко (но ещё в attackRange) — подходим ближе
                targetPosition = _enemy.Player.transform.position;
            }

            _movement.MoveTowards(targetPosition, speed, _enemy.ObstacleCheckDistance, _enemy.ObstacleLayer);
        }

        public void Exit()
        {
            _movement.Stop();
        }
    }
}