using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 _targetPos;
    private Vector3 _direction;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _targetPos = GetRandomPointerInCircle();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (_enemy.IsChasing)
        {
            _enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        _direction = (_targetPos - _enemy.transform.position).normalized;
        
        _enemy.MoveEnemy(_direction * _enemy.MovementSpeed);

        if ((_enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointerInCircle();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    private Vector3 GetRandomPointerInCircle()
    {
        return _enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * _enemy.RandomMovementRange;
    }
}
