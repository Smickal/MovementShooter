using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GunContainer : MonoBehaviour
{
    InventoryType currentInventoryType;
    InventoryType lastState;

    [Space(5)]
    [Header("GunAttributes")]
    [SerializeField] Transform _primaryGunHolder;
    [SerializeField] Transform _secondaryGunHolder;
    [SerializeField] Transform _knifeGunHolder;
    [SerializeField] Weapon[] _weapons;

    [Space(5)]
    [Header("Reference")]
    [SerializeField] AmmoReserveText _ammoReserveText;



    private void Start()
    {
        StartInventoryState(InventoryType.Secondary);
    }

    private void Update()
    {
        SetInventoryState();

    }

    private void SetInventoryState()
    {
        lastState = currentInventoryType;
        currentInventoryType = InputReader.Instance.InventoryState;


        if(lastState != currentInventoryType)
        {
            StartInventoryState(currentInventoryType);
        }
    }


    private void StartInventoryState(InventoryType inventoryType)
    {
        DeactivateAllGuns();
        ActivateAGun(inventoryType);


    }

    private void DeactivateAllGuns()
    {
        foreach(Weapon weapon in _weapons)
        {
            if(weapon != null)
            {
                weapon.UnSubscribeText();
                weapon.gameObject.SetActive(false);
            }
        }
    }

    private void ActivateAGun(InventoryType inventoryType)
    {
        foreach (Weapon weapon in _weapons)
        {
            if (weapon == null) continue;
            if(weapon.InventoryType == inventoryType)
            {         
                if(weapon.IsTextActivated)
                {
                    weapon.SubscribeText(_ammoReserveText);
                    _ammoReserveText.ActivateAmmoCountText();
                }
                else
                {
                    _ammoReserveText.DeactivateAmmoCountText();
                }

                weapon.gameObject.SetActive(true);
                break;
            }
        }
    }
}
