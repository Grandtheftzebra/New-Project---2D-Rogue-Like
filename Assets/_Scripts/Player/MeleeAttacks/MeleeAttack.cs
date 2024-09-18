using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _damage;
    private int _hashedMeleeTrigger = Animator.StringToHash("IsMeleeAttacking");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        _animator.SetTrigger(_hashedMeleeTrigger);
        
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
            enemy.TakeDamage();
    }*/
}
