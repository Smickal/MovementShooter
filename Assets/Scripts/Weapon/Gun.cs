using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Weapon
{
    [SerializeField] protected bool isUnlimitedAmmo = false;

    [Header("GunAttributes")]
    [SerializeField] protected GunType _gunType;
    [SerializeField] protected bool _isSemiautomatic;

    [SerializeField] protected float _equipGunTime;
    [SerializeField] protected float _gunReloadTime;
    [SerializeField] protected float _gunDamage;
    [SerializeField] protected float _forceFeedBackValue;
    [SerializeField] protected float _forceFeedForwardValue;
    [SerializeField] protected float _gunRange;

    [Space(5)]
    [Header("AmmoAttributes")]
    [SerializeField] protected float _gunRateOfFire;
    [SerializeField] protected int _maxAmmoReserveInMag;
    [SerializeField] protected int _maxAmmoReserveInInvetory;
    protected int ammoInMag;
    protected int ammoInInvetory;

    [Space(5)]
    [Header("Reference")]
    [SerializeField] WeaponCasing _bulletCasingObj;
    [SerializeField] Transform _bulletEjectLocation;
    [SerializeField] int _bulletCasePool;
    List<WeaponCasing> _bulletCasingObjs = new List<WeaponCasing>();

    [Space(5)]
    [SerializeField] WeaponParticleSplash _gunSplashParticle;
    [SerializeField] Transform _gunSplashLocation;
    [SerializeField] int _gunSplashPool;
    List<WeaponParticleSplash> _splashObjs = new List<WeaponParticleSplash>();

    protected bool isGunActivated = false;
   
    public event Action OnGunEnable;
   
    public abstract void Shoot();
    public abstract void Reload();
    protected void ActivateGun() => isGunActivated = true;
    protected void DeactivateGun() => isGunActivated = false;

    protected void CreateBulletCasingPool()
    {
        for(int i = 0; i < _bulletCasePool; i++)
        {
            WeaponCasing newCasing = Instantiate(_bulletCasingObj);
            _bulletCasingObjs.Add(newCasing);
            newCasing.gameObject.SetActive(false);
        }
    }

    protected void CreateGunSplashPool()
    {
        for (int i = 0; i < _bulletCasePool; i++)
        {
            WeaponParticleSplash newSplash = Instantiate(_gunSplashParticle, _gunSplashLocation);
            newSplash.transform.localPosition = Vector3.zero;
            _splashObjs.Add(newSplash);
            newSplash.gameObject.SetActive(false);
        }
    }


    public void ActivateABulletCasingFromPool()
    {
        foreach(WeaponCasing bulletCasing in _bulletCasingObjs)
        {
            if(bulletCasing.IsActivated == false)
            {
                bulletCasing.transform.position = _bulletEjectLocation.transform.position;
                bulletCasing.ActivateGameObject((transform.up + transform.right) * 2f);
                break;
            }
        }
    }

    public void ActivateAGunSplashFromPool()
    {
        foreach(WeaponParticleSplash bullletSplash in _splashObjs)
        {
            if(bullletSplash.IsActivated == false)
            {
                bullletSplash.ActivateGameObject();
                break;
            }
        }
    }

    protected void ReduceAmmoInMag()
    {
        if (ammoInMag == 0 || isUnlimitedAmmo) return;

        ammoInMag--;

        _reserveText.SetText(ammoInMag.ToString(), ammoInInvetory.ToString());
    }

    protected void ReduceAmmoInInventory()
    { 
        //if theres no ammo left in inventory or max ammo in mag
        if(ammoInInvetory == 0 || ammoInMag >= _maxAmmoReserveInMag) return;


        int difference = (_maxAmmoReserveInMag - ammoInMag);


        if(difference > ammoInInvetory)
        {
            ammoInMag += ammoInInvetory;
            ammoInInvetory = 0;
        }
        else
        {
            ammoInMag += difference;
            ammoInInvetory -= difference;
        }


        _reserveText.SetText(ammoInMag.ToString(), ammoInInvetory.ToString());
    }

}

public enum GunType
{
    Pistol,
    Rifle,
    Shotgun,
    Sniper
}

