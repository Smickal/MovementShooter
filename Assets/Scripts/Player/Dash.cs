using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("DashAttributes")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashDuration = 1f;


    [Header("Cooldown")]
    [SerializeField] private float dashCD;
    float dashCooldownTimer = 0f;

    [Header("Camera")]
    [SerializeField] float _extraDashCameraFOV = 5f;
    [SerializeField] float _dashFOVDuration = 0.25f;
    float currentCamFOV;

    [Space(5)]
    [Header("Reference")]
    [SerializeField] PlayerMovement _pm;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _orientation;
    [SerializeField] DashCooldownUI _ui;
    
    private Vector3 delayedForceToApply;

    private void Start()
    {
        InputReader.Instance.DashAction += EnterDash;
        dashCooldownTimer = dashCD;
    }

    private void Update()
    {
        if(_pm.IsDashing) StartDash();


        if(dashCooldownTimer < dashCD)
        {
            dashCooldownTimer += Time.deltaTime;
            _ui.ActivateDash1(dashCooldownTimer, dashCD);
        }
    }

    private void EnterDash()
    {
        if (dashCooldownTimer < dashCD) return;

        currentCamFOV = Camera.main.fieldOfView;

        _pm.IsDashing = true;
        _pm.camHandler.ActivateFovChange(currentCamFOV + _extraDashCameraFOV, _dashFOVDuration);
    }

    private void StartDash()
    {
        if (dashCooldownTimer < dashCD) return;
        else dashCooldownTimer = 0f;

        _rb.useGravity = false;

        float horizontalInput = InputReader.Instance.MovementValue.x;
        float verticalInput = InputReader.Instance.MovementValue.y;

        Vector3 forceToApply = Vector3.zero;

        if (horizontalInput == 0f && verticalInput == 0f)
        {
            forceToApply = _orientation.forward * _dashForce + _orientation.up * _dashUpwardForce;
        }
        else
        {
            forceToApply = _orientation.forward * verticalInput * _dashForce
                + _orientation.right * horizontalInput * _dashForce; 
        }


        _rb.velocity = Vector3.zero;

        _rb.AddForce(forceToApply, ForceMode.Impulse);
        Invoke(nameof(ResetDash), _dashDuration);
    }

    private void ResetDash()
    {
        _pm.IsDashing = false;
        _rb.useGravity = true;
        _pm.camHandler.ResetFOV();
    }
}
