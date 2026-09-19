using Assets.Scripts.EnemyControllers;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class EnemyAttackState : IState
    {
        private readonly BaseEnemy _enemy;
        private readonly EnemyAimController _aim;
        private readonly Weapon _weapon;
        private readonly float _windupTime;
        private readonly float _recoveryTime;

        private float _timer;
        private enum Phase { Windup, Shoot, Recovery }
        private Phase _currentPhase;

        public EnemyAttackState(BaseEnemy enemy, EnemyAimController aim, Weapon weapon, float windupTime, float recoveryTime)
        {
            _enemy = enemy;
            _aim = aim;
            _weapon = weapon;
            _windupTime = windupTime;
            _recoveryTime = recoveryTime;
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
                    if (_timer >= _recoveryTime)
                    {
                        _enemy.StateMachine.ChangeState(_enemy.ChaseState);
                    }
                    break;
            }
        }

        public void PhysicsUpdate() { }
        public void Exit() { }
    }
}
