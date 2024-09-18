using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape_Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject _bulletPrefab;
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Attack()
    {
       _animator.SetTrigger(ATTACK_HASH);

       if (transform.position.x > PlayerController.Instance.transform.position.x) // Flippt nach links wenn der Spieler links ist
           _spriteRenderer.flipX = true;
       else
           _spriteRenderer.flipX = false;
    }
    
    public void SpawnBulletEvent()
    {
        Instantiate(_bulletPrefab, this.transform.position, Quaternion.identity);
    }
}
