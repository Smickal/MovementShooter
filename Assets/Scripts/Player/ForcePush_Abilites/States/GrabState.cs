using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : ForcePushBaseState
{
    const float lerpRotationSpeed = 10f;
    const float lerpPositionSpeed = 10f;
    const float lerpSpeed = 100f;
    public GrabState(ForcePushStateMachine stateMachine) : base(stateMachine) { }

    Rigidbody itemRb;
    Transform itemTransform;
    Vector3 grabDirection;
    Camera mainCam;
    float firstDistance;

    bool isGrabbing = false;
    bool isGrabbedItemInPos = false;
    public override void Enter()
    {
        itemTransform = StateMachine.grabItemCollider.transform;
        itemRb = StateMachine.grabItemCollider.attachedRigidbody;
        mainCam = Camera.main;
       
    }

    public override void Tick(float deltaTime)
    {
        if (InputReader.Instance.IsGrabAbility)
        {
            //pull object towards player
            ActivateGrab();
        }
        else
        {
            //Drop Object
            StopGrabbing();
            StateMachine.SwitchState(new DetectionCheckState(StateMachine));
        }


        if (isGrabbedItemInPos)
        {
            //itemRb.velocity = Vector3.zero;
            //itemTransform.position = StateMachine.grabPos.position;
            //itemTransform.rotation = mainCam.transform.rotation;
        }


    }

    public override void FixedTick()
    {
        if(isGrabbing) Grabbing();
    }

    public override void Exit()
    {
        StateMachine.grabItemCollider = null;
    }


    private void ActivateGrab()
    {
        isGrabbing = true;
        itemRb.useGravity = false;

        firstDistance = Vector3.Distance(itemTransform.position, StateMachine.grabPos.position);
        itemRb.drag = 5;
    }


    private void Grabbing()
    {
        grabDirection = StateMachine.grabPos.position - itemTransform.position;

        float distanceBetweenGrab = grabDirection.magnitude;

        if (distanceBetweenGrab > 1f)
        {
            itemRb.AddForce(grabDirection * StateMachine.PullForce * distanceBetweenGrab, ForceMode.Force);
            isGrabbedItemInPos = false;
        }
        else
        {
            itemRb.velocity = Vector3.zero;
            itemRb.angularVelocity = Vector3.zero;

            Vector3 newPos = Vector3.Lerp(itemTransform.position, StateMachine.grabPos.position, Time.deltaTime * lerpSpeed);
            itemTransform.position = newPos;

            itemTransform.rotation = mainCam.transform.rotation;
            isGrabbedItemInPos = true;
        }

    }


    private void StopGrabbing()
    {
        isGrabbing = false;
        isGrabbedItemInPos= false;

        itemRb.useGravity = true;
        itemRb.drag = 0;
    }
}
