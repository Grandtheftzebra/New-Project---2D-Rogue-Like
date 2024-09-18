using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _collisionVFXPrefab;
    [SerializeField] private bool _isEnemyProjectile = false;
    [SerializeField] private float _projectileRange = 10f;

    private void Update()
    {
        transform.Translate(Vector2.right * (_moveSpeed * Time.deltaTime));
        DeleteProjectileWhenOutOfRange();
    }
    
    public void UpdateProjectileRange(float projectileRange)
    {
        _projectileRange = projectileRange;
    }
    
    public void UpdateProjectileSpeed(float projectileSpeed)
    {
        _moveSpeed = projectileSpeed;
    }
    
    private void DeleteProjectileWhenOutOfRange()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > _projectileRange)
            Destroy(gameObject);
        
    }

    /// <summary>
    /// Tries to get EnemyHealth, Indestructible or the Playerhealth class from the other object this projectile has collided with.
    /// First we check if we have collided with any of the above-mentioned things. We also make sure that it's collider is not marked as a trigger.
    /// Then we first check if we hit the player, if so we damage him and instantiate (spawn) the collision visiaul effects at that position then destroy the bullet.
    /// If we did not collide with the player and it has an Indestructible class on it this means we hit something that is not supposed to be destroyed by the bullet and
    /// we destroy the bullet while also spawning the collision vfx.
    /// </summary>
    /// <param name="other">The gameobject the projectile has collided with</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.GetComponent<Indestructible>();
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        
        if (!other.isTrigger && (enemy || indestructible || player)) // with !other.isTrigger we make sure we are not colliding with a trigger collider like the trees leaf collider
        {
            if ((player && _isEnemyProjectile) || (enemy && !_isEnemyProjectile))
            {
                player?.TakeDamage(1, transform);
                Instantiate(_collisionVFXPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);    
            }
            else if (!other.isTrigger && indestructible)
            {
                Instantiate(_collisionVFXPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}