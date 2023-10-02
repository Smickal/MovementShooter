using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushStateMachine : StateMachine
{
    [field: SerializeField] public float PushForce { get; private set; } = 20f;
    [field: SerializeField] public float MaxGrabDistance { get; private set; } = 10f;
    [field: SerializeField] public float PullForce { get; private set; } = 10f;
    [field: SerializeField] public float GrabForce { get; private set; } = 20f;
    [field: SerializeField] public LayerMask ObjectLayerMask { get; private set; }



    [field:Header("Reference")]
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public GunContainer GunContainer { get; private set; }
    [field: SerializeField] public Transform cameraPos { get; private set; }
    [field: SerializeField] public Transform grabPos { get; private set; }



    [HideInInspector] public bool IsGrabbing { get; set; }
    [HideInInspector] public Collider grabItemCollider;
    private void Start()
    {
        SwitchState(new DetectionCheckState(this));
    }
}
