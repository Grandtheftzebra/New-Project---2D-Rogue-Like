using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    private float _damage;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        _damage = (currentActiveWeapon as IWeapon).GetWeaponInfo().WeaponDamage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth _enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.TakeDamage(_damage); // // The ?(null-conditional) operator checks if _enemyHealth is not null. If it is not null, it executes TakeDamage.

    }
}
