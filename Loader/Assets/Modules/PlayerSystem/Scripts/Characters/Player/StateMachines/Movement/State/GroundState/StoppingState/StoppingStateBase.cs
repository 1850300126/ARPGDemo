using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoppingStateBase : PlayerGroundedState
{
    public StoppingStateBase(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;

        base.OnEnter();

        StartAnimation(movement_state_machine.player.animation_data.StoppingParameterHash);
        
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(movement_state_machine.player.animation_data.StoppingParameterHash);
    }

    protected override void AddInputAction()
    {
        base.AddInputAction();

        movement_state_machine.player.player_input.player_actions.HardAttack.started += OnHardAttackStarted;

        movement_state_machine.player.player_input.player_actions.LightAttack.started += OnLightAttackStarted;

        movement_state_machine.player.player_input.player_actions.Dodge.started += OnDodgeStarted;

        movement_state_machine.player.player_input.player_actions.Movement.started += OnMovementStarted;
    }

    protected override void RemoveInputAction()
    {
        base.RemoveInputAction();

        movement_state_machine.player.player_input.player_actions.HardAttack.started -= OnHardAttackStarted;

        movement_state_machine.player.player_input.player_actions.LightAttack.started -= OnLightAttackStarted;

        movement_state_machine.player.player_input.player_actions.Dodge.started -= OnDodgeStarted;

        movement_state_machine.player.player_input.player_actions.Movement.started -= OnMovementStarted;
    }       
    public override void OnFixUpdate()
    {
        base.OnFixUpdate();

        RotateTowardsTargetRotation();

        if (!IsMovingHorizontally())
        {
            return;
        }

        DecelerateHorizontally();
    }        
    public override void OnAnimationTransitionEvent()
    {   
        Debug.Log(111);
        movement_state_machine.ChangeState(movement_state_machine.idle_state);
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
}
