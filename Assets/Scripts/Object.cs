using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour, IGrabAble
{
    [Header("Reference")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] Outline _outline;



    public void ActivateOutline()
    {
        _outline.enabled = true;
    }

    public void DeactivateOutline()
    {
        _outline.enabled = false;
    }

}
