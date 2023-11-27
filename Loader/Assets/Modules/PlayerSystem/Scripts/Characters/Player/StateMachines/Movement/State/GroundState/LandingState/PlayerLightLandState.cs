using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightLandState : PlayerLandingState
{
    public PlayerLightLandState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        movement_state_machine.reusable_data.MovementSpeedModifier = 0;

        base.OnEnter();

        ResetVelocity();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();

        if (!IsMovingHorizontally())
        {
            return;
        }

        ResetVelocity();
    }
    public override void OnAnimationTransitionEvent()
    {        
        
        if (movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            movement_state_machine.ChangeState(movement_state_machine.idle_state);

            return;
        }

        OnMove();
    }    
}
