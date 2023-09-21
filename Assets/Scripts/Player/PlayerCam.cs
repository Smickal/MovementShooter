using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float startingCameraFOV = 90f;

    [Space(5)]
    [SerializeField] Transform _orientation;
    [SerializeField] Transform _cameraHolder;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] CinemachineRecomposer _CineRecomp;


    bool isTilting = false;
    float targetTilt;
    float lastTiltValue = 0f;

    bool isFOVChanged = false;
    float targetFOV;
    float lastFOV;

    Camera mainCamera;

    float tiltTimer;
    float fovTimer;

    float tiltDuration;
    float fovDuration;
    void Start()
    {
        mainCamera = Camera.main;
        _camera.m_Lens.FieldOfView = startingCameraFOV;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();

        if(isTilting) TiltCamera();
        if(isFOVChanged) ChangeFOV();
    }

    private void RotatePlayer()
    {
        Vector3 mainCamDir = mainCamera.transform.rotation.eulerAngles;
        _orientation.rotation = Quaternion.Euler(0f, mainCamDir.y, 0f);
    }


    public void ActivateTilt(float tiltDegree, float duration)
    {
        isTilting = true;
        targetTilt = tiltDegree;
        tiltDuration = duration;
        tiltTimer = 0f;

        Invoke(nameof(StopTilting), duration);
    }

    private void TiltCamera()
    {
        tiltTimer += Time.deltaTime;
        _CineRecomp.m_Dutch = Mathf.SmoothStep(lastTiltValue, targetTilt, tiltTimer / tiltDuration);
    }


    private void StopTilting()
    {
        isTilting = false;
        lastTiltValue = targetTilt;
    }

    public void ActivateFovChange(float fovDegree, float duration)
    {
        isFOVChanged = true;
        targetFOV = fovDegree;
        fovDuration = duration;
        lastFOV = _camera.m_Lens.FieldOfView;

        fovTimer = 0f;

        Invoke(nameof(StopFOV), fovDuration);
    }

    private void ChangeFOV()
    {
        fovTimer += Time.deltaTime;
        float cameraFOV = Mathf.SmoothStep(lastFOV, targetFOV, fovTimer / fovDuration);
        _camera.m_Lens.FieldOfView = cameraFOV;
    }


    private void StopFOV()
    {
        isFOVChanged = false;
    }


    public void ResetFOV()
    {
        ActivateFovChange(startingCameraFOV, 0.5f);
    }

    public void ResetTilt()
    {
        ActivateTilt(0, 0.5f);
    }

}
