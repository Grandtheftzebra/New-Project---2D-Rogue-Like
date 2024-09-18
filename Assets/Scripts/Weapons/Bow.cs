using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo _weaponInfo;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPoint;

    private Animator _anim;
    readonly int FIRE_HASH = Animator.StringToHash("Fire"); 
    // Hashes "Fire" to an int to be used in the _anim.SetTrigger(FIRE_HASH) method.
    // We do that because string comparison is heavy to calculate and we want to avoid it.
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
    }

    public WeaponInfo GetWeaponInfo() => _weaponInfo;

    public void Attack()
    {
        _anim.SetTrigger(FIRE_HASH);
        GameObject arrow = Instantiate(_arrowPrefab, _arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation); // ActiveWeapon.Instance.transform.rotation works because we have MouseFollow that has always the same rotation we the bow. Otherwise we would have to use Quaternion.identity
        arrow.GetComponent<Projectile>().UpdateProjectileRange(_weaponInfo.WeaponRange);
    }
}