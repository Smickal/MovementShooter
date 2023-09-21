using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerControls.IGamePlayActions
{
    private static InputReader instance;
    public static InputReader Instance { get { return instance; } }

    public PlayerControls PlayerControls;

    private Vector2 movementValue;
    public Vector2 LookValue;
    private InventoryType inventoryState = InventoryType.Primary;

    public event Action JumpAction;

    public event Action SlidingAction;
    public event Action StopSlidingAction;

    public event Action InteractAction;

    public event Action AttackAction;
    public event Action StopAttackAction;

    public event Action ReloadAction;

    bool isSliding = false;
    bool isAttack = false;

    public bool IsSliding { get { return isSliding; } }
    public bool IsAttack { get { return isAttack; } }   
    public Vector2 MovementValue { get { return movementValue; }}
    public InventoryType InventoryState { get { return inventoryState; } }

    private void Awake()
    {
        if(instance != null && instance != this) Destroy(this.gameObject);
        else instance = this;

        PlayerControls = new PlayerControls();
        PlayerControls.GamePlay.SetCallbacks(this);
    }


    private void OnEnable()
    {
        PlayerControls.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAttack = true;
            AttackAction?.Invoke();

        }         

        if (context.canceled)
        {
            isAttack = false;
            StopAttackAction?.Invoke();
        }

    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ReloadAction?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementValue =  context.ReadValue<Vector2>();
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        //if (!context.performed) return;

        //Debug.Log(context.ToString());

        if (context.performed)
        {
            SlidingAction?.Invoke();
            isSliding = true;
        }

        if (context.canceled)
        {
            StopSlidingAction?.Invoke();
            isSliding = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
            InteractAction?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
            JumpAction?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookValue = context.ReadValue<Vector2>();
    }

    public void OnPrimaryInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        inventoryState = InventoryType.Primary;
    }

    public void OnSecondaryInventory(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        inventoryState = InventoryType.Secondary;
    }

    public void OnKnifeInventory(InputAction.CallbackContext context)
    {
        if(context.performed) return;
        inventoryState = InventoryType.Knife;
    }

   
}
