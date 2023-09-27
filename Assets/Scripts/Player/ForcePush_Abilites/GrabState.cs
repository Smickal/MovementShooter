using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GrabState : ForcePushBaseState
{
    public GrabState(ForcePushStateMachine stateMachine) : base(stateMachine) { }

    Vector3 boxCastSize = Vector3.one;

    RaycastHit[] boxCastHit;
    RaycastHit singleRayHit;

    Camera mainCam;
    Vector3 cameraPos;
    float maxGrabDetectionRange;

    IGrabAble currentGrabItem;
    IGrabAble lastGrabItem;
    LayerMask objectLayerMask;
    public override void Enter()
    {
        mainCam = Camera.main;
        cameraPos = StateMachine.cameraPos.position;
        maxGrabDetectionRange = StateMachine.MaxGrabDistance;
        objectLayerMask = StateMachine.ObjectLayerMask;
    }

    public override void Tick(float deltaTime)
    {
        GetClosestGrabObject();
        ActivateAndDeactivateOutlines();
    }


    public override void FixedTick()
    {
        
    }

    public override void Exit()
    {
        
    }

    
    private void GetClosestGrabObject()
    {
        boxCastHit = Physics.BoxCastAll(mainCam.transform.position, boxCastSize, mainCam.transform.forward, Quaternion.identity, maxGrabDetectionRange, objectLayerMask);
        Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out singleRayHit, maxGrabDetectionRange, objectLayerMask);

        Vector3 rayHitPoint = Vector3.zero;
        if (singleRayHit.collider != null)
        {
            rayHitPoint = singleRayHit.point;
        }
        else
        {
            rayHitPoint = mainCam.transform.position + mainCam.transform.forward * maxGrabDetectionRange;
        }

        float closestDistance = float.MaxValue;
        foreach (RaycastHit hit in boxCastHit)
        {
            IGrabAble grabAble = hit.collider.gameObject.GetComponent<IGrabAble>();
            if (hit.collider.attachedRigidbody == null || grabAble == null) continue;

            Vector3 hitLoc = hit.collider.gameObject.transform.position;
            Debug.DrawLine(mainCam.transform.position, hitLoc, Color.green);

            float distanceBetweenPoint = Vector3.Distance(hitLoc, rayHitPoint);
            if (distanceBetweenPoint < closestDistance)
            {
                closestDistance = distanceBetweenPoint;
                StateMachine.closestCollider = hit.collider;
            }
        }


        if (boxCastHit.Length == 0)
        {
            StateMachine.closestCollider = null;
            currentGrabItem?.DeactivateOutline();
            
            if(lastGrabItem != null)
            {
                lastGrabItem.DeactivateOutline();
                lastGrabItem = null;
            }
        }

        if (StateMachine.closestCollider != null)
        {
            Debug.DrawLine(mainCam.transform.position, StateMachine.closestCollider.transform.position, Color.red);
        }

        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward, Color.black);
    }

    private void ActivateAndDeactivateOutlines()
    {
        if (StateMachine.closestCollider == null)
        {
            return;
        }
        currentGrabItem = StateMachine.closestCollider.GetComponent<IGrabAble>();

        if (currentGrabItem != lastGrabItem)
        {
            lastGrabItem?.DeactivateOutline();
            currentGrabItem.ActivateOutline();

            lastGrabItem = currentGrabItem;
        }
    }

}
