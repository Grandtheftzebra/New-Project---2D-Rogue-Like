using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float _enemyHealth;
    [SerializeField] float _deathTriggerTimer;

    [SerializeField] float _knockBackThrust;
    Knockback _knockback;

    FlashWhenHit _flashWhenHit;

    [SerializeField] GameObject _deathVFXPrefab;

   void Awake()
    {
        _flashWhenHit = GetComponent<FlashWhenHit>();
        _knockback = GetComponent<Knockback>();
    }

    public float GetHealth() => _enemyHealth;

    public void TakeDamage(float damage)
    {
        _enemyHealth -= damage;
     
        _knockback.ApplyKnockBack(PlayerController.Instance.transform, _knockBackThrust);
        StartCoroutine(_flashWhenHit.FlashRoutine());

        if (_enemyHealth <= 0)
            StartCoroutine(DieRoutine());
    }

    void Die()
    {
        Instantiate(_deathVFXPrefab, transform.position, Quaternion.identity);
        GetComponent<PickupSpawner>().SpawnItem();
        Destroy(gameObject);
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(_deathTriggerTimer);
        Die();
    }
}
