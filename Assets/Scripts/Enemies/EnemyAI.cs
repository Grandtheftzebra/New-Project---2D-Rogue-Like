using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _roamChangeDir = 2f;
    [SerializeField] private float _aggroRange = 0f;
    [SerializeField] private MonoBehaviour _enemyType; // Need Monobehaviour to later change the easily the type of enemy, for example when we have a close range enemy instead of a shooter we just change prefabs and the functionality stays the same. 
    [SerializeField] private float _attackCooldown; // Need Monobehaviour to later change the easily the type of enemy, for example when we have a close range enemy instead of a shooter we just change prefabs and the functionality stays the same. 
    [SerializeField] private bool _canMoveWhileAttacking = false;
    
    private bool _canAttack = true;
    private Vector2 _roamingPosition;

    private enum State
    {
        Roaming,
        Attacking
    }

    private float _timeRoaming;
    private State _state;
    private EnemyPathfinding _enemyPathfinding;

    private void Awake()
    {
        _enemyPathfinding = GetComponent<EnemyPathfinding>();
        _state = State.Roaming;
    }


    private void Start()
    {
        _roamingPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (_state)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Roaming()
    {
        _timeRoaming += Time.deltaTime;
        
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < _aggroRange)
        {
            _state = State.Attacking;
            return;
        }
        
        if (_timeRoaming >= _roamChangeDir)
            _roamingPosition = GetRoamingPosition();
        
        _enemyPathfinding.MoveTo(_roamingPosition);
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > _aggroRange)
            _state = State.Roaming;
        
        if ( _aggroRange != 0 && _canAttack)
            StartCoroutine(AttackCDRoutine());
        
        if (!_canMoveWhileAttacking)
            _enemyPathfinding.StopMoving();
        else
            _enemyPathfinding.MoveTo(_roamingPosition);
    }

    private IEnumerator AttackCDRoutine()
    {
        _canAttack = false;
        (_enemyType as IEnemy)?.Attack(); 
        
        yield return new WaitForSeconds(_attackCooldown);
        
        _canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        _timeRoaming = 0;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // normalized so it doesn't move faster sideways
    }
}