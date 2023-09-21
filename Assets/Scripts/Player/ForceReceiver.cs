using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] Rigidbody rb;



    public void AddExternalForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }    
}
