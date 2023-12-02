using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightAttackFinishState : AttackFinishState
{
    public LightAttackFinishState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();

        // movement_state_machine.player.animator.CrossFade("LightAttackFinish", 0.1f);
    }
    public override void OnExit()
    {
        base.OnExit();
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnLightAttackStarted(InputAction.CallbackContext context)
    {        
        JugdeComboFinish();
        movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
    }     

    protected override void OnHardAttackStarted(InputAction.CallbackContext context)
    {
        movement_state_machine.ChangeState(movement_state_machine.hard_attack_state);
    } 
}
