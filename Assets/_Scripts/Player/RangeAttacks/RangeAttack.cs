using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] private Projectile _bloodPrefab;
    [SerializeField] private Transform _muzzle;

    public void Shoot()
    {
        Projectile bloodProjectile = Instantiate(_bloodPrefab, _muzzle.position, quaternion.identity);
        bloodProjectile.GetComponent<Rigidbody2D>().AddForce(_muzzle.up * _bloodPrefab.FireForce, ForceMode2D.Impulse);
    }
}
