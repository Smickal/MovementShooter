using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class DebugText : MonoBehaviour
{
    [SerializeField] protected bool IsActivated;
    [SerializeField] protected TMP_Text _debugText;


    public abstract void SetText(string text);
}
