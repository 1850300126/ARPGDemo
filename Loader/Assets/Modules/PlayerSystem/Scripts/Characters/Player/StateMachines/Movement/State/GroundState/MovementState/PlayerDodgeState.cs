using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDodgeState : PlayerGroundedState
{
    private bool shouldKeepRotating;
    public PlayerDodgeState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }
    public override void OnEnter()
    {
        
        movement_state_machine.reusable_data.MovementSpeedModifier = grounded_data.DodgeData.SpeedModifier;

        base.OnEnter();

        StartAnimation(movement_state_machine.player.animation_data.DodgeParameterHash);

        movement_state_machine.reusable_data.current_jump_force = airborne_data.JumpData.StrongForce;

        movement_state_machine.reusable_data.RotationData = grounded_data.DodgeData.RotationData;

        Dash();

        shouldKeepRotating = movement_state_machine.reusable_data.movement_input != Vector2.zero;

        MsgSystem.instance.SendMsg("player_dodege", null);
    }
    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(movement_state_machine.player.animation_data.DodgeParameterHash);

        SetBaseRotationData();
    }        
    public override void OnFixUpdate()
    {
        Float();

        if (!shouldKeepRotating)
        {
            return;
        }

        RotateTowardsTargetRotation();
    }
    public override void OnAnimationTransitionEvent()
    {   
        ResetVelocity();

        if (movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            movement_state_machine.ChangeState(movement_state_machine.idle_state);

            return;
        }

        movement_state_machine.ChangeState(movement_state_machine.sprint_state);
    }

    protected override void AddInputAction()
    {
        base.AddInputAction();

        movement_state_machine.player.player_input.player_actions.Movement.performed += OnMovementPerformed;
    }

    protected override void RemoveInputAction()
    {
        base.RemoveInputAction();

        movement_state_machine.player.player_input.player_actions.Movement.performed -= OnMovementPerformed;
    }

    protected override void OnMovementPerformed(InputAction.CallbackContext context)
    {
        base.OnMovementPerformed(context);

        shouldKeepRotating = true;
    }
    private void Dash()
    {
        Vector3 dashDirection = movement_state_machine.player.transform.forward;

        dashDirection.y = 0f;

        UpdateTargetRotation(dashDirection, false);

        if (movement_state_machine.reusable_data.movement_input != Vector2.zero)
        {
            UpdateTargetRotation(GetMovementInputDirection());

            dashDirection = GetTargetRotationDirection(movement_state_machine.reusable_data.CurrentTargetRotation.y);
        }

        movement_state_machine.player.player_rb.velocity = dashDirection * GetMovementSpeed(false);        
    }
}
