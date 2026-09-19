using Assets.Scripts.EnemyControllers;
using Assets.Scripts.States;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(EnemyMovementController))]
    [RequireComponent(typeof(EnemyAimController))]
    public class EnemyRabbit : BaseEnemy
    {
        [Header("Attack Settings")]
        [SerializeField] private float _attackRange = 8f;
        [SerializeField] private float _windupTime = 0.4f;
        [SerializeField] private float _recoveryTime = 0.6f;
        [SerializeField] private float _preferredDistance = 4f;

        private EnemyMovementController _movementController;
        private EnemyAimController _aimController;
        private Weapon _weapon;

        private void Awake()
        {
            BaseAwake();

            _movementController = GetComponent<EnemyMovementController>();
            _aimController = GetComponent<EnemyAimController>();
            _weapon = GetComponentInChildren<Weapon>();

            ChaseState = new EnemyChaseState(this, _movementController, _attackRange);
            //AttackState = new EnemyAttackState(this, _aimController, _weapon, _windupTime, _recoveryTime);
            AttackState = AttackState = new EnemyMoveAndShootState( this, _movementController, _aimController, _weapon, _windupTime, _recoveryTime, _attackRange, _preferredDistance);

            StateMachine.Initialize(ChaseState);
        }
    }
}
