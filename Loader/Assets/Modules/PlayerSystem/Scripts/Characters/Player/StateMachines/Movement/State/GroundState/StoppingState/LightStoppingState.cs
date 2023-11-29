using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStoppingState : StoppingStateBase
{
    public LightStoppingState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();

        movement_state_machine.reusable_data.MovementDecelerationForce = 6f;
        
        StartAnimation(movement_state_machine.player.animation_data.MediumStopParameterHash);
        
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(movement_state_machine.player.animation_data.MediumStopParameterHash);
    }
}
