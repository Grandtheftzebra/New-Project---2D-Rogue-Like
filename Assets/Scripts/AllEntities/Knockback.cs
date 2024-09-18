using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Knockback : MonoBehaviour
{
    //[SerializeField] public float _knockBackThrust; Don't like! Put it to enemyHealth to have more control of the thrust effect of each enemy
    [SerializeField] float _knockBackTime = .2f;
    public bool IsKnockedBack { get; private set; }

    Rigidbody2D _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
     
    }

    public void ApplyKnockBack(Transform damageSource, float knockBackThrust) 
    {
        IsKnockedBack = true;
        Vector2 _force = (transform.position - damageSource.position).normalized * knockBackThrust * _rb.mass;
        
        _rb.AddForce(_force, ForceMode2D.Impulse);
        StartCoroutine(DisableKnockBackRoutine());
    }

    IEnumerator DisableKnockBackRoutine()
    {
        yield return new WaitForSeconds(_knockBackTime);
        _rb.velocity = Vector2.zero; // Remove it's velocity to make it behave normal again
        IsKnockedBack = false;
    }
}
