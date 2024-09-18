using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int _activeSlotIndex = 0;

    private PlayerControls _playerControls;

    protected override void Awake()
    {
        base.Awake();
        
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        _playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }
    
    public void EquipInitialWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        _activeSlotIndex = indexNum;

        foreach (Transform inventorySlot in this.transform)
            inventorySlot.GetChild(0).gameObject.SetActive(false);

        this.transform.GetChild(_activeSlotIndex).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (PlayerHealth.Instance.IsDead)
            return;
        
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        
        Transform childTransform = transform.GetChild(_activeSlotIndex);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        
        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.NoWeaponInSlot();
            return;
        }
        
        GameObject _selectedWeapon = weaponInfo.WeaponPrefab;
        
        GameObject _newWeapon = Instantiate(_selectedWeapon, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0); // Fixes quaternion.euler issue with bow rotation
        _newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.EquipNewWeapon(_newWeapon.GetComponent<MonoBehaviour>());
    }
}