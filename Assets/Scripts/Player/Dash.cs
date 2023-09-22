using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("DashAttributes")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashCooldown = 1f;
    float dashTimer = 0f;

    [Space(5)]
    [Header("Reference")]
    [SerializeField] PlayerMovement _pm;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _orientation;

    bool isDashActivated = false;

    private void Start()
    {
        InputReader.Instance.DashAction += StartDash;
    }

    private void Update()
    {
        if(isDashActivated)
        {
            dashTimer += Time.deltaTime;

            if(dashTimer >= _dashCooldown)
            {
                StopDash();
            }
        }
    }

    private void StartDash()
    {
        if (isDashActivated) return;

        dashTimer = 0f;
        isDashActivated = true;

        Vector2 movementDirInput = InputReader.Instance.MovementValue;

        float horizontalInput = movementDirInput.x;
        float verticalInput = movementDirInput.y;

        _pm.IsDragActivated = false;

        if (horizontalInput == 0f && verticalInput == 0f)
        {          
            _rb.AddForce(_orientation.forward * _dashForce, ForceMode.Impulse);
        }
        else
        {
            
            Vector3 dashDir = verticalInput * _dashForce *_orientation.forward + horizontalInput * _dashForce * _orientation.right;

            _rb.AddForce(dashDir, ForceMode.Impulse);
        }


    }

    private void StopDash()
    {
        _pm.IsDragActivated = true;
        isDashActivated = false;
    }
}
