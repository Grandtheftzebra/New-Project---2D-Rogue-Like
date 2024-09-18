using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ImprovedTimers;
using StateMachine;
using StateMachine.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using IState = StateMachine.IState;
using Timer = ImprovedTimers.Timer;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : Entity
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    public int FacingDirectionValue = 1;
    
    [Header("Interaction")] 
    [SerializeField] private float _interactionRadius = 1f;
    [SerializeField] private Vector3 _triggerOffset;
    public bool CanInteract { get; set; } = false; // TODO: Check if this can be made private set

    [Header("Dash")] 
    [SerializeField] private float _dashDuration = 0.1f;
    [SerializeField] private float _dashCooldown = 5f;
    [SerializeField] private float _dashVelocityAmount;
    private float dashVelocity = 1f;

    [Header("Melee")] 
    [SerializeField] private float _meleeCooldown = 0.5f;
    public bool CanMeleeAttack { get; private set; } = true;
    
    
    // Components
    [SerializeField] private InputReader _inputReader;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private RangeAttack _rangeAttack;
    private MeleeAttack _meleeAttack;

    private List<Timer> _timers;
    
    private CountDownTimer _dashPerformedTimer;
    private CountDownTimer _dashCooldownTimer;

    private CountDownTimer _meleeAttackTimer;
    
    // State Machine
    private StateMachine.StateMachine _stateMachine;
    
    public override void Awake()
    {
        base.Awake();
        
        // Components
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rangeAttack = GetComponentInChildren<RangeAttack>();
        _meleeAttack = GetComponentInChildren<MeleeAttack>();

        // Dash Timer
        _dashPerformedTimer = new CountDownTimer(_dashDuration);
        _dashCooldownTimer = new CountDownTimer(_dashCooldown);
        _dashPerformedTimer.OnTimerStart += () => dashVelocity = _dashVelocityAmount;
        _dashPerformedTimer.OnTimerStop += () =>
        {
            dashVelocity = 1f;
            _dashCooldownTimer.Start();
        };
        
        // Melee Attack Timer
        _meleeAttackTimer = new CountDownTimer(_meleeCooldown);
        _meleeAttackTimer.OnTimerStop += () => CanMeleeAttack = true;
        
        _timers = new List<Timer>(3) { _dashPerformedTimer, _dashCooldownTimer, _meleeAttackTimer };
        
        // State Machine 
        _stateMachine = new StateMachine.StateMachine();
        
        // States
        LocomotionState locomotionState = new LocomotionState(this, _animator);
        DashState dashState = new DashState(this, _animator);
        
        // Transitions
        At(dashState, locomotionState, new FuncPredicate(() => _dashPerformedTimer.IsFinished )); 
        Any(dashState, new FuncPredicate(() => _dashPerformedTimer.IsRunning )); // Need a state to populate the dictionary with at least one state. Without this line the code throws an error
        
        // Start state
        _stateMachine.SetState(locomotionState); // Throws an error when not at least one Transition State is implemented.
    }

    private void OnEnable() // Maybe refactor this later and put the entire Interaction into the locomotion state!
    {
        _inputReader.Interact += OnInteract;
        _inputReader.Dash += OnDash;
        _inputReader.RangeAttack += OnShoot;
        _inputReader.MeleeAttack += OnMeleeAttack;
    }

    private void OnDisable()
    {
        _inputReader.Interact -= OnInteract;
        _inputReader.Dash -= OnDash;
        _inputReader.RangeAttack -= OnShoot;
        _inputReader.MeleeAttack -= OnMeleeAttack;
    }

   
    public override void Update()
    {
        base.Update();
        
        _stateMachine.Update();
        
        if (Input.GetKeyDown(KeyCode.F))
            Debug.Log(Stats.ToString());
    }
    
    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    #region StateMachine

    private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);

    private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

    #endregion

    #region Movement and Viewpoint
    
    public void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputReader.Movement * (_moveSpeed * dashVelocity * Time.fixedDeltaTime));
        CalculateFacingDirectionValue();
    }

    private void CalculateFacingDirectionValue()
    {
        if (_inputReader.Movement.x > 0)
            FacingDirectionValue = 1;
        else if(_inputReader.Movement.x < 0)
            FacingDirectionValue = -1;
    }
    
    // TODO If this feels weird, we can simply but this method onto the DirectionIndicator object, which then rotates instead of the player
    public void MakePlayerLookAtMousePosition() 
    {
        Vector2 mousePos = _inputReader.MousePos;

        Vector2 aimDir = mousePos - _rb.position;
        float aimAngle = Mathf.Atan2(aimDir.y,aimDir.x) * Mathf.Rad2Deg - 90;
        _rb.rotation = aimAngle;
        
        Debug.DrawLine(transform.position, mousePos, Color.red, Time.deltaTime);
    }
    
    #endregion

    #region Dash
    private void OnDash() 
    {
        if (_dashCooldownTimer.IsRunning)
            return;
        
        _dashPerformedTimer.Start();
    }
    
    #endregion

    #region Interactions // Maybe move Interactions in a seperate class
    
    private void OnInteract()
    {
        if (!CanInteract)
            return;
        
        Collider2D[] collisions = Physics2D.OverlapCircleAll(
            transform.position + (_triggerOffset * FacingDirectionValue), 
            _interactionRadius);

        foreach (var hit in collisions) 
        { 
            hit.gameObject.TryGetComponent(out IInteractable entity);
            entity?.Interact();
        }
        
    }

    #endregion

    private void OnShoot()
    {
        _rangeAttack.Shoot();
    }

    private void OnMeleeAttack()
    {
        if (!CanMeleeAttack)
            return;
        
        CanMeleeAttack = false;
        
        _meleeAttack.Attack();
        
        _meleeAttackTimer.Start();
    }
    private void OnDrawGizmos()
    {
        Vector3 offset = Vector3.right;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (_triggerOffset * FacingDirectionValue), _interactionRadius);
    }
}
