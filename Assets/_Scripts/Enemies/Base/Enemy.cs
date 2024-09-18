using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IMovable, ITriggerCheckable
{
    [SerializeField] private EnemySO enemyData;

    #region State Machine
    protected EnemyStateMachine stateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyChaseState ChaseState { get; set; }

    #endregion

    public float RandomMovementRange = 5f;
    public float MovementSpeed = 1;

    #region Event Functions

    

    public void Awake()
    {
        stateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, stateMachine);
        ChaseState = new EnemyChaseState(this, stateMachine);
        AttackState = new EnemyAttackState(this, stateMachine);
        
    }
    
    private void Start()
    {
        stateMachine.Initialize(IdleState);
        RB = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    #endregion

    #region IMovable
    public Rigidbody2D RB { get; set; }
    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
    }
    #endregion

    #region ITriggerCheckable
    
    public bool IsChasing { get; set; }
    public bool IsAttacking { get; set; }
    
    #endregion

}
