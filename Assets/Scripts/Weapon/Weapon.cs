using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected int ShootAnimationHash = Animator.StringToHash("Shoot");
    protected int ReloadAnimationHash = Animator.StringToHash("Reload");
    protected int EnterGunAnimationHash = Animator.StringToHash("Equip");
    protected int AttackAnimationHash = Animator.StringToHash("Attack");
    
    [Header("Inventory Type")]
    [Space(5)]
    [SerializeField] bool _isTextActivated;
    [SerializeField] protected InventoryType _inventoryType;

    [Space(5)]
    [Header("Animator")]
    [SerializeField] protected Animator _gunAnimator;



    protected AmmoReserveText _reserveText;
    protected ForcePushStateMachine stateMachine;

    public bool IsTextActivated { get { return _isTextActivated; } }
    public InventoryType InventoryType { get { return _inventoryType; } }

    public bool IsWeaponActivated { get; private set; } = false;


    protected void PlayEnterAnimation() => _gunAnimator.SetTrigger(EnterGunAnimationHash);
    protected void PlayShootAnimation() => _gunAnimator.SetTrigger(ShootAnimationHash);
    protected void PlayReloadAnimation() => _gunAnimator.SetTrigger(ReloadAnimationHash);
    public void ActivateWeapon() => IsWeaponActivated = true;
    public  void DeactivateWeapon() => IsWeaponActivated = false;

    public void SubscribeText(AmmoReserveText text)
    {
        _reserveText = text;
    }

    public void SubscribeForcePushStateMachine(ForcePushStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void UnSubscribeText()
    {
        _reserveText = null;
    }

    public void SetDownWeapon()
    {
        DeactivateWeapon();


    }
}

public enum InventoryType
{
    Primary,
    Secondary,
    Knife
}
