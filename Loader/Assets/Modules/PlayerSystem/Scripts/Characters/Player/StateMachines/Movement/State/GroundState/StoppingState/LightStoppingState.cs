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

        movement_state_machine.reusable_data.MovementDecelerationForce = 5f;
        
        movement_state_machine.player.PlayAnimation("LightStop", null, 1, false, 0.1f);
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
