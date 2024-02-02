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
        
        movement_state_machine.player.PlayAnimation("LightStop", null, 0.1f);
        // movement_state_machine.player.TimelinePlayer.CtrlPlayable.CrossFade("LightStop", 0.25f);
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
