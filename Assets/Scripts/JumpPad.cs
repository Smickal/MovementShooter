using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpPadForce;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.attachedRigidbody;

        if (rb != null)
        {
           rb.AddForce(Vector3.up * jumpPadForce, ForceMode.Impulse);
        }
    }
}
