using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    State currentState;

    private void Update()
    {
        currentState.Tick(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        currentState.FixedTick();
    }

    public void SwitchState(State nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState.Enter();
    }

}
