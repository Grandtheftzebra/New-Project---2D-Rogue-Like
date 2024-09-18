using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy _enemy;
    protected EnemyStateMachine _enemyStateMachine;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        _enemy = enemy;
        _enemyStateMachine = enemyStateMachine;
    }
    
    public virtual void EnterState() {}
    public virtual void ExitState(){}
    public virtual void FrameUpdate() {}
    public virtual void PhysicsUpdate() {}
    public virtual void AnimationTriggerEvent() {}
}