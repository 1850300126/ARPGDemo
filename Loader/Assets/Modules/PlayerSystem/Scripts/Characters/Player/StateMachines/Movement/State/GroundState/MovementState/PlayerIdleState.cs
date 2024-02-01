using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        movement_state_machine.player.PlayAnimation("Idle", null, 1, false, 0.1f);
        // movement_state_machine.player.TimelinePlayer.CtrlPlayable.CrossFade("Idle", 0.25f);

        movement_state_machine.reusable_data.MovementSpeedModifier = 0;

        movement_state_machine.reusable_data.current_jump_force = airborne_data.JumpData.StationaryForce;

        ResetVelocity();
    }
    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }
    public override void OnUpdate()
    {   
        base.OnUpdate();

        SpiritAdd(Time.deltaTime * 5);

        if(movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            return;
        }

        OnMove();
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
    public override void OnExit()
    {
        // StopAnimation(movement_state_machine.player.animation_data.IdleParameterHash);

        base.OnExit();
    }
    protected override void AddInputAction()
    {
        base.AddInputAction();

        movement_state_machine.player.player_input.player_actions.Dodge.started += OnDodgeStarted;
        
        movement_state_machine.player.player_input.player_actions.HardAttack.started += OnHardAttackStarted;

        movement_state_machine.player.player_input.player_actions.LightAttack.started += OnLightAttackStarted;

    }

    protected override void RemoveInputAction()
    {
        base.RemoveInputAction();

        movement_state_machine.player.player_input.player_actions.Dodge.started -= OnDodgeStarted;
        
        movement_state_machine.player.player_input.player_actions.HardAttack.started -= OnHardAttackStarted;

        movement_state_machine.player.player_input.player_actions.LightAttack.started -= OnLightAttackStarted;
    }
}
