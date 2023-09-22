using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [SerializeField] float wallRunningForce;
    [SerializeField] float wallRunningTime;
    [SerializeField] float wallCheckDistance = 0.7f;
    [SerializeField] float minGroundDistance = 1f;

    [Header("WalljumpForce")]
    [SerializeField] float wallJumpUpForce;
    [SerializeField] float wallJumpSideForce;
    [SerializeField] float wallJumpForwardForce = 3f;
    [SerializeField] float groundForceMultiplier = 1.2f;

    float wallRunningTimer;

    [Space(5)]
    [SerializeField] Transform _orientation;
    [SerializeField] LayerMask _wallLayerMask;

    [Space(5)]
    [Header("CameraExtraAttributes")]
    [SerializeField] private float tilt;
    [SerializeField] private float tiltDuration = 0.5f;
    [SerializeField] private float extraFOV;
    [SerializeField] private float fovDuration = 0.5f;
    float startFOV;
    float targetFOV;


    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;


    private RaycastHit leftWall;
    private RaycastHit rightWall;
    private RaycastHit groundHit;

    bool isLeftWall;
    bool isRightWall;
    bool isAboveGround;

    Rigidbody rb;
    PlayerMovement pm;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        InputReader.Instance.JumpAction += WallJump;

        startFOV = Camera.main.fieldOfView;
        targetFOV = startFOV + extraFOV;
    }

    private void Update()
    {
        isLeftWall = Physics.Raycast(transform.position, -_orientation.right, out leftWall, wallCheckDistance, _wallLayerMask);
        isRightWall = Physics.Raycast(transform.position, _orientation.right, out rightWall, wallCheckDistance, _wallLayerMask);

        isAboveGround = !Physics.Raycast(transform.position, Vector3.down, out groundHit, minGroundDistance);


        //check for wall run criteria
        if((isLeftWall || isRightWall) && rb.velocity.magnitude > 0f && isAboveGround && 
            (InputReader.Instance.MovementValue.y > 0.1f) && 
            !exitingWall)
        {
            StartWallRunning();
        }


        //Exiting Wall
        else if(exitingWall)
        {
            if (pm.IsWallRunning)
                StopWallRunning();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;
            
            if(exitWallTimer <= 0)
                exitingWall = false;
        }

        //
        else
        {
            if (pm.IsWallRunning)
                StopWallRunning();
        }

    }


    private void FixedUpdate()
    {
        if (pm.IsWallRunning) 
            WallRunningMovement();
    }

    private void StartWallRunning()
    {
        if (pm.IsWallRunning) return;

        wallRunningTimer = wallRunningTime;


        pm.IsWallRunning = true;   
        groundForceMultiplier = 1f;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        if (isLeftWall) pm.camHandler.ActivateTilt(-tilt, tiltDuration);
        else if (isRightWall) pm.camHandler.ActivateTilt(tilt, tiltDuration);

        pm.camHandler.ActivateFovChange(targetFOV, fovDuration);
    }


    private void WallRunningMovement()
    {
        //Debug.Log($"leftWAll = {isLeftWall}, RightWall = {isRightWall}");

        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = isRightWall ? rightWall.normal : leftWall.normal;
        Vector3 wallProjectedDirection = Vector3.Cross(wallNormal, transform.up);

        if ((_orientation.forward - wallProjectedDirection).magnitude > (_orientation.forward - -wallProjectedDirection).magnitude)
        {
            wallProjectedDirection = -wallProjectedDirection;
        }

       
        rb.AddForce(wallProjectedDirection * wallRunningForce, ForceMode.Force);

        //Debug.Log(rb.angularVelocity);
        
        groundForceMultiplier += Time.deltaTime;
        wallRunningTimer -= Time.deltaTime;
        if (wallRunningTimer < 0) StopWallRunning();
    }


    private void StopWallRunning()
    {
        if (pm.IsWallRunning == false) return;

        rb.useGravity = true;
        pm.IsWallRunning = false;
        pm.camHandler.ActivateTilt(0, tiltDuration);
        pm.camHandler.ResetFOV();
    }

    private void WallJump()
    {
        if (!pm.IsWallRunning) return;

        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = isRightWall ? rightWall.normal : leftWall.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallJumpSideForce * wallNormal;


        //Add JumpForce
        // rb.velocity = new Vector3(rb.velocity.x, 0f, rb.position.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

    }
}
