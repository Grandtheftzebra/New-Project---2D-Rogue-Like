using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "New Weapon")]
public class WeaponInfo : ScriptableObject
{
    public GameObject WeaponPrefab;
    public float WeaponCooldown;
    public float WeaponDamage;
    public float WeaponRange;
}
