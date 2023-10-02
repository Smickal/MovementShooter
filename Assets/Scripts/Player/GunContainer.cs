using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GunContainer : MonoBehaviour
{
    const float gunHolderRotation = 30f;
    const float timeToRotateGun = 0.1f;

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
    [SerializeField] Transform _weaponHolders;
    [SerializeField] ForcePushStateMachine stateMachine;

    Weapon currentWeaponActivated;
    Coroutine currentRoutine;

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
                weapon.SubscribeForcePushStateMachine(stateMachine);
                weapon.gameObject.SetActive(true);
                currentWeaponActivated = weapon;
                break;
            }
        }
    }

    public void DisableCurrentWeapon()
    {
        if(currentWeaponActivated ==  null) return;

        currentWeaponActivated.DeactivateWeapon();

        if(currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine =  StartCoroutine(StartRotating(gunHolderRotation));
    }

    public void ActiveCurrentWeapon()
    {
        if (currentWeaponActivated == null) return;

        currentWeaponActivated.ActivateWeapon();

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine =  StartCoroutine(StartRotating(0));
    }


    IEnumerator StartRotating(float rotation)
    {
        float timer = 0f;
        Vector3 before = Vector3.zero;

        while(timer <= timeToRotateGun)
        {

            timer += Time.deltaTime;
            _weaponHolders.localRotation = Quaternion.Lerp(Quaternion.Euler(before), Quaternion.Euler(new Vector3(rotation, 0f, 0f)), timer / timeToRotateGun);
            yield return null;
        }

    }
}
