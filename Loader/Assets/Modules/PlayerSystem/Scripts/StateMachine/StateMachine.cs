using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState current_state;

    public void ChangeState(IState new_state)
    {
        current_state?.OnExit();

        current_state = new_state;

        current_state.OnEnter();
    }

    public void HandleInput()
    {
        current_state?.OnHandleInput();
    }

    public void Update()
    {
        current_state?.OnUpdate();
    }

    public void FixUpdate()
    {
        current_state?.OnFixUpdate();
    }

    public void OnTriggerEnter(Collider collider)
    {
        current_state?.OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        current_state?.OnTriggerExit(collider);
    }

    public void OnAnimationEnterEvent()
    {
        current_state?.OnAnimationEnterEvent();
    }

    public void OnAnimationExitEvent()
    {
        current_state?.OnAnimationExitEvent();
    }

    public void OnAnimationTransitionEvent()
    {
        current_state?.OnAnimationTransitionEvent();
    }
}


