using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForcePushBaseState :State
{
    public ForcePushStateMachine StateMachine;

    public ForcePushBaseState(ForcePushStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
}
