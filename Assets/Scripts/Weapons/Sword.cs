using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _slashAnimPrefab;
    private GameObject _slashAnim;

    [SerializeField] private WeaponInfo _weaponInfo;
    // [SerializeField] private float _attackCooldown; no need for that, we have it in WeaponInfo ScriptableObject
    private Animator _myAnimator;

    private Transform _weaponCollider;
    private Transform _slashAnimSpawnPoint;

    public void Awake()
    {
        _myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _weaponCollider = PlayerController.Instance.GetWeaponCollider();
        _slashAnimSpawnPoint = PlayerController.Instance.GetSlashAnimSpawnPoint();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        _myAnimator.SetTrigger("Attack");
        _weaponCollider.gameObject.SetActive(true);

        _slashAnim = Instantiate(_slashAnimPrefab, _slashAnimSpawnPoint.position, Quaternion.identity);
        _slashAnim.transform.parent = this.transform.parent;
        // Not quite understand what this does, maybe watch tutorial again Video 16: Sword Visual Feedback | Course: Unity2D RPG: Complete Combat System
        // Intructor Explanation: That way we know our swords parent, is the active weapon in our hierarchy and we want to set our slashanimation prefab 
        // that we just instantiated to the same parent, the active weapon, in our hierarchy.
        
    }
    
    public WeaponInfo GetWeaponInfo() => _weaponInfo;
    
    private void TurnOffWeaponColliderAnimEvent()
    {
        _weaponCollider.gameObject.SetActive(false);
    }

    public void FlipSwingUpAnimEvent()
    {
        _slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController.Instance.FacingLeft)
            _slashAnim.GetComponent<SpriteRenderer>().flipX = true;
    }

    public void FlipSwingDownAnimEvent()
    {
        _slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
            _slashAnim.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 _mousePosition = Input.mousePosition;
        Vector3 _playerScreenPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
    
        float _swordAngle =
            Mathf.Atan2(_mousePosition.y, _mousePosition.x) *
            Mathf.Rad2Deg; // This will move our sword a little as we move our mouse over the screen.
    
        if (_mousePosition.x < _playerScreenPosition.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, _swordAngle);
            _weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, _swordAngle);
            _weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}