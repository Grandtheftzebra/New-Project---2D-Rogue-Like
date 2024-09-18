using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] GameObject _destroyVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DealDamage>() || other.gameObject.GetComponent<Projectile>() || other.gameObject.GetComponent<MagicLaser>())
        {
            PickupSpawner pickupSpawner = GetComponent<PickupSpawner>();
            pickupSpawner?.SpawnItem();
            Instantiate(_destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
            
    }
}
