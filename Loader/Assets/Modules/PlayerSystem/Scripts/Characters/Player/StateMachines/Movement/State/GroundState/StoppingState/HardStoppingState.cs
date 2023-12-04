using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardStoppingState : StoppingStateBase
{
    public HardStoppingState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();

        movement_state_machine.reusable_data.MovementDecelerationForce = 2f;

        movement_state_machine.player.animator.CrossFade("HardStop", 0.1f);
        // StartAnimation(movement_state_machine.player.animation_data.HardStopParameterHash);
        
    }

    public override void OnExit()
    {
        base.OnExit();

        // StopAnimation(movement_state_machine.player.animation_data.HardStopParameterHash);
    }
}
