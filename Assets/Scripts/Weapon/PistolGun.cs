using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolGun : Gun
{
    float fireRateTimer = 0;
    bool isReadyToFire = true;
    bool isFireRateCoolDownActivated = false;
    bool isReloading = false;

    RaycastHit hit;
    Camera mainCam;

    private void Awake()
    {
        ammoInMag = _maxAmmoReserveInMag;
        ammoInInvetory = _maxAmmoReserveInInvetory;
    }

    private void OnEnable()
    {
        PlayEnterAnimation();
        Invoke(nameof(ActivateGun), _equipGunTime);

        _reserveText.SetText(ammoInMag.ToString(), ammoInInvetory.ToString());

        if(_isSemiautomatic) InputReader.Instance.AttackAction += Shoot;
        InputReader.Instance.ReloadAction += Reload;

        isReadyToFire = true;
        fireRateTimer = _gunRateOfFire;
    }


    private void Start()
    {
        CreateBulletCasingPool();
        CreateGunSplashPool();

        mainCam = Camera.main;
    }

    private void Update()
    {
        FireRateGunCooldown();
        FullAutoCheck();
    }
    private void OnDisable()
    {
        DeactivateGun();

        if(_isSemiautomatic) InputReader.Instance.AttackAction -= Shoot;
        InputReader.Instance.ReloadAction -= Reload;
    }



    private void FireRateGunCooldown()
    {
        if (isFireRateCoolDownActivated)
        {
            fireRateTimer -= Time.deltaTime;

            if (fireRateTimer < 0f)
            {
                isReadyToFire = true;
                isFireRateCoolDownActivated = false;
            }
        }
    }

    private void FullAutoCheck()
    {
        if (_isSemiautomatic == false && InputReader.Instance.IsAttack == true)
        {
            Shoot();
        }
    }


    public override void Shoot()
    {
        if (isGunActivated == false || isReadyToFire == false || isReloading == true || ammoInMag == 0) return;

        //Activate Firerate Cooldown
        fireRateTimer = _gunRateOfFire;
        isReadyToFire = false;
        isFireRateCoolDownActivated = true;
        
        
        //check Raycast
        Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, _gunRange);

        //Apply Force to Objects with RigidBodies
        if(hit.collider != null)
        {
            Rigidbody hitRigid = hit.collider.attachedRigidbody;
            if(hitRigid)
            {
                hitRigid.AddForce((hit.point - mainCam.transform.position).normalized * _forceFeedForwardValue, ForceMode.Impulse);
            }
        }

        //Reduce Ammo Count
        ReduceAmmoInMag();


        //Do Damage to Damageable



        // Play Animation
        PlayShootAnimation();
    }

    public override void Reload()
    {
        if ((isGunActivated == false && isReadyToFire == false && isReloading == true) 
            || ammoInInvetory == 0 || (_maxAmmoReserveInMag - ammoInMag) == 0) return;

        //Tries to Add ammo to mag
        ReduceAmmoInInventory();

        isReloading = true;
        PlayReloadAnimation();

        Invoke(nameof(DoneReload), _gunReloadTime);
    }


    private void DoneReload()
    {
        isReloading = false;
    }
}
