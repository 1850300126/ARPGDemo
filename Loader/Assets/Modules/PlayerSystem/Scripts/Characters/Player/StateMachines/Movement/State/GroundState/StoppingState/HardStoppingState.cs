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

        movement_state_machine.player.PlayAnimation("HardStop", null, 1, false, 0.25f);
        // movement_state_machine.player.TimelinePlayer.CtrlPlayable.CrossFade("HardStop", 0.25f);
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
