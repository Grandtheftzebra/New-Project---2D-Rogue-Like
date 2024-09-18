using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo _weaponInfo;
    [SerializeField] private GameObject _magicLaserPrefab;
    [SerializeField] private Transform _laserSpawnPoint;

    private Animator _anim;
    
    readonly int SPELL_HASH = Animator.StringToHash("CastLaser");
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    
    void Start()
    {
    }

    void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        _anim.SetTrigger(SPELL_HASH);
    }
    
    public void SpawnMagicLaserEvent()
    {
        GameObject magicLaser = Instantiate(_magicLaserPrefab, _laserSpawnPoint.position, Quaternion.identity);
        magicLaser.GetComponent<MagicLaser>().UpdateLaserRange(_weaponInfo.WeaponRange);
    }
    
    public WeaponInfo GetWeaponInfo() => _weaponInfo;

    /// <summary>
    /// Get the current mousePos and the players screen position. We need both these variables to determine if the mouse
    /// is behind the player or in front of him. The Staff Angle holds the z value with which we will make the weapon move based on our mouse movement
    /// In the if we adjust the weapon accordingly. If the cursor is more leftsided the weapon has to be as well and vice versa for the right side
    /// </summary>
    private void MouseFollowWithOffset()
    {
        Vector3 _mousePosition = Input.mousePosition;
        Vector3 _playerScreenPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float staffAngle = Mathf.Atan2(_mousePosition.y, _mousePosition.x) * Mathf.Rad2Deg;
        // This will move our sword a little as we move our mouse over the screen.

        if (_mousePosition.x < _playerScreenPosition.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, staffAngle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, staffAngle);
        }
    }
}