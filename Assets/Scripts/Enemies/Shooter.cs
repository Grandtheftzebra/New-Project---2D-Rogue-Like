using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _burstAmount;
    [SerializeField] private int _projectilesPerBurst;
    [SerializeField] [Range(0, 359)] private int _angleSpread;

    [SerializeField] private float _timeBetweenBoursts;
    [SerializeField] private float _restTime = 1f;
    [SerializeField] private float _bulletSpawnOffset = 0.1f;
    [SerializeField] private bool _stagger;
    [Tooltip("Stagger has to be true for oscillate to work.")]
    [SerializeField] private bool _oscillate;
    
    private bool _isShooting = false;

    private void OnValidate()
    {
        if (_bulletSpeed < 0.1f) {_bulletSpeed = 0.1f;}
        if (_burstAmount < 1) {_burstAmount = 1;}
        if (_projectilesPerBurst < 1) {_projectilesPerBurst = 1;}
        if (_timeBetweenBoursts < 0.1f) {_timeBetweenBoursts = 0.1f;}
        if (_restTime < 0.1f) {_restTime = 0.1f;}
        if (_bulletSpawnOffset < 0.1f) {_bulletSpawnOffset = 0.1f;}
        if (_angleSpread == 0) {_angleSpread = 0;}
        if (_oscillate) {_stagger = true;}
        if (!_oscillate) {_stagger = false;}
    }

    public void Attack()
    {
        if (!_isShooting)
            StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        _isShooting = true;

        float timeBetweenProjectiles = 0f;
        //This is our cone of influence
        TargetConeOfInfluence(out var startAngle, out var currentAngle, out var angleStep, out var endAngle);
        
        if (_stagger)
            timeBetweenProjectiles = _timeBetweenBoursts / _projectilesPerBurst;

        for (int i = 0; i < _burstAmount; i++)
        {
            if (!_oscillate)
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

            if (_oscillate && i % 2 == 0)
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            else if (_oscillate)
            {
                // We reverse the start and end angle and the angle step to make the oscillation
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;    
            }
            
            
            for (int j = 0; j < _projectilesPerBurst; j++)
            {
                Vector2 bulletSpawnPosition = FindBulletSpawnPosition(currentAngle);

                GameObject bullet = Instantiate(_bulletPrefab, bulletSpawnPosition, Quaternion.identity);
                bullet.transform.right = bullet.transform.position - transform.position; // transform.position is the position of the object that the script is attached to. In this case, the shooter.
                // bullet.GetComponent<Projectile>().UpdateProjectileSpeed(_bulletSpeed); this works but we use a different approach for learning purposes. The out keyword!

                if (bullet.TryGetComponent<Projectile>(out Projectile projectile))
                    projectile.UpdateProjectileSpeed(_bulletSpeed);
        
                currentAngle += angleStep;
                
                if (_stagger)
                    yield return new WaitForSeconds(timeBetweenProjectiles);
            }
            
            currentAngle = startAngle;
            
            if (!_stagger)
                yield return new WaitForSeconds(_timeBetweenBoursts);
        }

        yield return new WaitForSeconds(_restTime);

        _isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle - _angleSpread / 2;
        currentAngle = startAngle;
        endAngle = targetAngle + _angleSpread / 2;
        float halfAngleSpread = 0f;
        angleStep = 0f;

        if (_angleSpread != 0)
        {
            angleStep = _angleSpread / (_projectilesPerBurst - 1);
            halfAngleSpread = _angleSpread / 2;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private void Shoot(Vector2 bulletSpawnPosition)
    {
       
    }
    
    private Vector2 FindBulletSpawnPosition(float currentAngle)
    {
        float x = transform.position.x + _bulletSpawnOffset * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + _bulletSpawnOffset * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
    
        Vector2 position = new Vector2(x, y);
    
        return position;
    }
    
}