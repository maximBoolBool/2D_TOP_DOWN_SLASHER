using Assets.Scripts.EnemyControllers;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class EnemyChaseState : IState
    {
        private readonly BaseEnemy _enemy;
        private readonly EnemyMovementController _movement;
        private readonly float _attackRange;

        public EnemyChaseState(BaseEnemy enemy, EnemyMovementController movement, float attackRange)
        {
            _enemy = enemy;
            _movement = movement;
            _attackRange = attackRange;
        }

        public void Enter() { }

        public void LogicUpdate()
        {
            if (_enemy.Player == null) return;

            float distance = Vector2.Distance(_enemy.transform.position, _enemy.Player.transform.position);

            // Если игрок в зоне атаки — переходим в состояние атаки
            if (distance <= _attackRange)
            {
                _enemy.StateMachine.ChangeState(_enemy.AttackState);
            }
        }

        public void PhysicsUpdate()
        {
            if (_enemy.Player == null) return;

            float speed = _enemy.UnitCharacteristic.ActualCharacteristics[CharecteristicType.Speed];
            _movement.MoveTowards(_enemy.Player.transform.position, speed, _enemy.ObstacleCheckDistance, _enemy.ObstacleLayer);
        }

        public void Exit()
        {
            _movement.Stop();
        }
    }
}
