using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpPadForce;

    private void OnCollisionEnter(Collision collision)
    {
        ForceReceiver forceReceiver = collision.collider.GetComponent<ForceReceiver>();

        if (forceReceiver != null)
        {
            forceReceiver.AddExternalForce(Vector3.up * jumpPadForce);
        }
    }
}
