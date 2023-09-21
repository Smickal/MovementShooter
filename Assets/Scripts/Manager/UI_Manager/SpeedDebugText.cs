using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDebugText : DebugText
{
    public override void SetText(string text)
    {
        if (!IsActivated)
        {
            _debugText.SetText(string.Empty);
            return;
        }

        _debugText.SetText("Speed: " + text);
    }
}
