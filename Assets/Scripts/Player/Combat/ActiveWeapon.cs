using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls _playerControls;

    private float _attackCooldownTime;
    
    // safes us one line of code, when variables start with the same value we can just seperate them
    // by a comma and then they still count as standalone
    private bool _attackButtonPressed, _isAttacking = false;
    


    protected override void Awake()
    {
        base.Awake();

        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void Start()
    {
        _playerControls.Combat.Attack.started +=
            _ => StartAttacking(); // Anonymous function in C# also called lambda, the underscore indicates we are not passing anything through as an argument for our attack function!
        // Commonly used for such matters in C# and Python for example!
        _playerControls.Combat.Attack.canceled +=
            _ => StopAttacking(); // canceled is basically just on mouse button up, so when we stop pressing the left mouse button.

        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void EquipNewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        _attackCooldownTime = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().WeaponCooldown;
        AttackCooldown();
    }

    public void NoWeaponInSlot() => CurrentActiveWeapon = null;

    private void StartAttacking() => _attackButtonPressed = true;

    private void StopAttacking() => _attackButtonPressed = false;

    private void AttackCooldown()
    {
        _isAttacking = true;
        StopAllCoroutines(); // This will stop all coroutines that are running, so we don't have multiple coroutines running at the same time. And we just have one running in that class so it is okay!
        StartCoroutine(AttackCooldownRoutine());
    }
    
    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(_attackCooldownTime);
        _isAttacking = false;
    }

    private void Attack()
    {
        if (_attackButtonPressed && !_isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
            // This is a cast, we are casting/calling our current active weapon to an IWeapon interface,
            // so we can call the attack function from the IWeapon interface.
        }
    }
}