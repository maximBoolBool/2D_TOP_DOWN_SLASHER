using UnityEngine;
using Assets.Scripts._Project.Code.Slasher.Core;
using Assets.Scripts._Project.Code.Slasher.Game.States;
using Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic;
using Assets.Scripts._Project.Code.Slasher.Game.StateMachines;

namespace Assets.Scripts._Project.Code.Slasher.Game.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        public EnemyStateMachine StateMachine { get; protected set; }
        public IState ChaseState { get; protected set; }
        public IState AttackState { get; protected set; }

        public UnitCharecteristic UnitCharacteristic { get; protected set; }
        public GameObject Player { get; protected set; }

        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float obstacleCheckDistance = 1.5f;

        public LayerMask ObstacleLayer => obstacleLayer;
        public float ObstacleCheckDistance => obstacleCheckDistance;

        protected void BaseAwake()
        {
            StateMachine = new EnemyStateMachine();
            UnitCharacteristic = GetComponent<UnitCharecteristic>();
            Player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER);
        }

        protected virtual void Update()
        {
            if (UnitCharacteristic == null || !UnitCharacteristic.IsAlive) return;
            StateMachine.CurrentState?.LogicUpdate();
        }

        protected virtual void FixedUpdate()
        {
            if (UnitCharacteristic == null || !UnitCharacteristic.IsAlive) return;
            StateMachine.CurrentState?.PhysicsUpdate();
        }
    }
}
