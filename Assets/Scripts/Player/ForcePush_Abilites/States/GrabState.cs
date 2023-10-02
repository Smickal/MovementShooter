using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : ForcePushBaseState
{
    const string ignorePlayerColMask = "IgnorePlayerCol";
    const float lerpRotationSpeed = 10f;
    const float lerpPositionSpeed = 10f;
    const float lerpSpeed = 100f;
    public GrabState(ForcePushStateMachine stateMachine) : base(stateMachine) { }

    Rigidbody itemRb;
    Transform itemTransform;
    Vector3 grabDirection;
    Camera mainCam;
    float firstDistance;

    bool isReadyToPush = false;
    string beforeLayerMask;

    public override void Enter()
    {
        itemTransform = StateMachine.grabItemCollider.transform;
        itemRb = StateMachine.grabItemCollider.attachedRigidbody;
        mainCam = Camera.main;

        beforeLayerMask = LayerMask.LayerToName(StateMachine.grabItemCollider.gameObject.layer);

        StateMachine.GunContainer.DisableCurrentWeapon();
        InputReader.Instance.AttackAction += ForcePushTheItem;

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
            InputReader.Instance.AttackAction -= ForcePushTheItem;
            StateMachine.SwitchState(new DetectionCheckState(StateMachine));
        }
    }

    public override void FixedTick()
    {
        if(StateMachine.IsGrabbing) Grabbing();
    }

    public override void Exit()
    {
        StateMachine.GunContainer.ActiveCurrentWeapon();
    }


    private void ActivateGrab()
    {
        StateMachine.IsGrabbing = true;
        itemRb.useGravity = false;
        itemRb.freezeRotation = true;

        firstDistance = Vector3.Distance(itemTransform.position, StateMachine.grabPos.position);
        itemRb.drag = 5;
    }


    private void Grabbing()
    {
        grabDirection = StateMachine.grabPos.position - itemTransform.position;

        float distanceBetweenGrab = grabDirection.magnitude;

        if (distanceBetweenGrab > 3f)
        {
            itemRb.AddForce(grabDirection * StateMachine.PullForce, ForceMode.Force);
        }
        else
        {
            itemRb.velocity = Vector3.zero;
            itemRb.angularVelocity = Vector3.zero;
            StateMachine.grabItemCollider.gameObject.layer = LayerMask.NameToLayer(ignorePlayerColMask);

            Vector3 newPos = Vector3.Lerp(itemTransform.position, StateMachine.grabPos.position, Time.deltaTime * lerpSpeed);
            itemTransform.position = newPos;

            itemTransform.rotation = mainCam.transform.rotation;
            isReadyToPush = true;
        }

    }


    private void StopGrabbing()
    {
        StateMachine.grabItemCollider.gameObject.layer = LayerMask.NameToLayer(beforeLayerMask);
        StateMachine.IsGrabbing = false;

        itemRb.freezeRotation= false;
        itemRb.useGravity = true;
        itemRb.drag = 0;

        StateMachine.grabItemCollider = null;
    }

    private void ForcePushTheItem()
    {
        if (!isReadyToPush) return;
        InputReader.Instance.AttackAction -= ForcePushTheItem;

        itemRb.AddForce(mainCam.transform.forward * StateMachine.PushForce, ForceMode.Impulse);
        StateMachine.InputReader.ForcePushed();
    }

}
