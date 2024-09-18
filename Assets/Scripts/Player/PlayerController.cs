using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Player Stats")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _dashSpeed = 2f;
    [SerializeField] float _dashTime = 0.2f;
    [SerializeField] float _dashCD = 2f;
    bool _isDashing = false;
    float _startingMoveSpeed;

    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] Transform _weaponCollider;
    [SerializeField] Transform _slashAnimSpawnPoint;
    public bool FacingLeft { get { return _facingLeft; } set { _facingLeft = value; } }
    bool _facingLeft = false;

    PlayerControls _playerControls;
    Vector2 _movement;

    Rigidbody2D _rb;
    Animator _myAnimator;
    SpriteRenderer _mySpriteRen;
    Knockback _knockback;


    protected override void Awake() 
    {
        base.Awake();

        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _mySpriteRen = GetComponent<SpriteRenderer>();
        _knockback = GetComponent<Knockback>();
    }

    void Start()
    {
        _playerControls.Combat.Dash.performed += _ => Dash();
        _startingMoveSpeed = _moveSpeed; // This is implemented so we don't have to divide in the dash method all the time which can lead to precision errors over time.
        
        ActiveInventory.Instance.EquipInitialWeapon();
    }

    void Update() // Good for player inputs
    {
        PlayerInput();
        
    }

    void FixedUpdate() // Good for physics
    {
        ControlPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return _weaponCollider;
    }

    public Transform GetSlashAnimSpawnPoint()
    {
        return _slashAnimSpawnPoint;
    }

    private void OnEnable()
    {
        _playerControls.Enable(); // Enables the new Input System.
    }
    
    private void OnDisable()
    {
        _playerControls.Disable(); // Disables the new Input System (automatically when the player is disabled.)
    }

    void PlayerInput()
    {
        _movement = _playerControls.Movement.Move.ReadValue<Vector2>(); // Reads our actionmap and action

        _myAnimator.SetFloat("moveX", _movement.x);
        _myAnimator.SetFloat("moveY", _movement.y);
    }

    void Move()
    {
        if (_knockback.IsKnockedBack || PlayerHealth.Instance.IsDead)
            return;
        
        _rb.MovePosition(_rb.position + _movement * (_moveSpeed * Time.fixedDeltaTime));
    }

    void ControlPlayerFacingDirection()
    {
        Vector3 _playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 _mousePosition = Input.mousePosition;

        if (_mousePosition.x < _playerScreenPoint.x)
        {
            _mySpriteRen.flipX = true;
            FacingLeft = true;
        }
        else
        {
            _mySpriteRen.flipX = false;
            FacingLeft = false;
        }
    }

    void Dash()
    {
        bool hasStamina = PlayerStamina.Instance.CurrentStamina > 0;
        
        if (!_isDashing && hasStamina)
        {
            PlayerStamina.Instance.UseStamina(1);
            _trailRenderer.emitting = true;
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        _isDashing = true;
        _moveSpeed *= _dashSpeed;

        yield return new WaitForSeconds(_dashTime);

        //_moveSpeed /= _dashSpeed;
        _moveSpeed = _startingMoveSpeed;
        _trailRenderer.emitting = false;

        yield return new WaitForSeconds(_dashCD);
        _isDashing = false;
    }
}
