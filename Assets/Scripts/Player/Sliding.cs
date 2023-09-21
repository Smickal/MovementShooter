using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Camera Attributes")]
    [SerializeField] private float extraFovTarget = 10f;
    [SerializeField] private float fovDuration = 0.5f;
    float startFOV;
    float targetFOV;


    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;

        InputReader.Instance.SlidingAction += StartSlide;
        InputReader.Instance.StopSlidingAction += StopSlide;
        InputReader.Instance.StopSlidingAction += ResizePlayer;

        startFOV = Camera.main.fieldOfView;
        targetFOV = startFOV + extraFovTarget;
    }

    private void Update()
    {
        horizontalInput = InputReader.Instance.MovementValue.x;
        verticalInput = InputReader.Instance.MovementValue.y;
    }

    private void FixedUpdate()
    {
        if (pm.IsSliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        pm.IsSliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;

        pm.camHandler.ActivateFovChange(targetFOV, fovDuration);
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        // sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.IsSliding = false;

        pm.camHandler.ResetFOV();
    }

    private void ResizePlayer()
    {
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
