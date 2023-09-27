using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushStateMachine : StateMachine
{
    [field:SerializeField] public float MaxGrabDistance { get; private set; } = 10f;
    [field: SerializeField] public LayerMask ObjectLayerMask { get; private set; }  



    [field:Header("Reference")]
    [field: SerializeField] public Transform cameraPos { get; private set; }

    [HideInInspector] public Collider closestCollider;
    private void Start()
    {
        SwitchState(new GrabState(this));
    }
}
