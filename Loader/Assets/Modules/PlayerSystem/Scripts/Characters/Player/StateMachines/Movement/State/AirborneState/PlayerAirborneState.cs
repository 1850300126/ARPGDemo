using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(movement_state_machine.player.animation_data.AirborneParameterHash);

        ResetAttackIndex();
    }        
    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        StopAnimation(movement_state_machine.player.animation_data.AirborneParameterHash);
    }
}
