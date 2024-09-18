using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
    public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private float _knockBackThrust;
    [SerializeField] private float _invincibleFrameTime = .5f;
    private int _currentHealth;
    private bool _canTakeDamage = true;
    public bool IsDead { get; private set; }
    
    private Knockback _knockback;
    private FlashWhenHit _flashWhenHit;
    
    private Slider _healthSlider;
    const string HEALTH_BAR_TEXT = "Health_Bar";
    const string TOWN_SCENE_TEXT = "Town";
    readonly int  DEATH_HASH = Animator.StringToHash("Death");
    private float _timeBeforeReloadAfterDeath = 2f;
    
    protected override void Awake()
    {
        base.Awake();
        
        _knockback = GetComponent<Knockback>();
        _flashWhenHit = GetComponent<FlashWhenHit>();
    }

    void Start()
    {
        IsDead = false;
        _currentHealth = _maxHealth;
        
        UpdateHealthSlider();
    }

    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.collider.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform); // get rid of magic number later
        }
    }
    
    public void HealPlayer(int healAmount)
    {
        if (_currentHealth < _maxHealth)
        {
            _currentHealth += healAmount;
            UpdateHealthSlider();
        }
        
    }
    
    public void TakeDamage(int damage, Transform hitTransform)
    {
        if (!_canTakeDamage)
            return;
        
        ScreenShakeManager.Instance.ShakeScreen();
        _canTakeDamage = false;
        StartCoroutine(InvincibleFrameRoutine());
        
        _knockback.ApplyKnockBack(hitTransform, _knockBackThrust);
        StartCoroutine(_flashWhenHit.FlashRoutine());
        
        _currentHealth -= damage;
        
        if (_currentHealth <= 0 && !IsDead)
            Die();
        
        UpdateHealthSlider();
        
    }
    
    private IEnumerator InvincibleFrameRoutine()
    {
        yield return new WaitForSeconds(_invincibleFrameTime);
        _canTakeDamage = true;
    }
    
    private void Die()
    {
        _currentHealth = 0;
        IsDead = true;
        Destroy(ActiveWeapon.Instance.gameObject); // Destroy the active weapon when the player dies, so he can't attack while dead
        GetComponent<Animator>().SetTrigger(DEATH_HASH);
        
        StartCoroutine(LoadTownSceneOnDeathRoutine());
    }

    private IEnumerator LoadTownSceneOnDeathRoutine()
    {
        yield return new WaitForSeconds(_timeBeforeReloadAfterDeath);
        Destroy(gameObject);
        PlayerStamina.Instance.RefillStaminaOnPlayerDeath();
        SceneManager.LoadScene(TOWN_SCENE_TEXT);
    }

    public void UpdateHealthSlider()
    {
        if (_healthSlider == null)
            _healthSlider = GameObject.Find(HEALTH_BAR_TEXT).GetComponent<Slider>();
        
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _currentHealth;
    }
}
