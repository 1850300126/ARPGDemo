using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    private bool shouldKeepRotating;
    private bool can_start_fall;

    public PlayerJumpState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    #region IState
    public override void OnAnimationEnterEvent()
    {
        
    }

    public override void OnAnimationExitEvent()
    {
        
    }

    public override void OnAnimationTransitionEvent()
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();

        movement_state_machine.player.PlayAnimation("Jump", null, 1, false, 0.1f);
        // movement_state_machine.player.TimelinePlayer.CtrlPlayable.CrossFade("Jump", 0.25f);
        movement_state_machine.reusable_data.MovementSpeedModifier = 0;

        shouldKeepRotating = movement_state_machine.reusable_data.movement_input != Vector2.zero;

        movement_state_machine.reusable_data.TimeToReachTargetRotation = airborne_data.JumpData.RotationData.TargetRotationReachTime;

        Jump();
    }

    public override void OnExit()
    {
        base.OnExit();
            
        can_start_fall = false;
    }
    public override void OnUpdate() 
    {   
        base.OnUpdate();

        if (!can_start_fall && IsMovingUp(0f))
        {
            can_start_fall = true;
        }

        if (!can_start_fall || IsMovingUp(0f))
        {
            return;
        }

        movement_state_machine.ChangeState(movement_state_machine.fall_state);
    }        
    public override void OnFixUpdate()
    {
        base.OnFixUpdate();

        if (shouldKeepRotating)
        {
            RotateTowardsTargetRotation();
        }

        if (IsMovingUp())
        {
            DecelerateVertically();
        }
    }
    #endregion

    public void Jump()
    {               
        Vector3 _jump_force = movement_state_machine.reusable_data.current_jump_force;

        Vector3 jumpDirection = movement_state_machine.player.transform.forward;

        if (shouldKeepRotating)
        {
            UpdateTargetRotation(GetMovementDirection());

            jumpDirection = GetTargetRotationDirection(movement_state_machine.reusable_data.CurrentTargetRotation.y);
        }

        _jump_force.x *= jumpDirection.x;
        _jump_force.z *= jumpDirection.z;

        // _jump_force = GetJumpForceOnSlope(jumpForce);

        ResetVelocity();
        
        movement_state_machine.player.player_rb.AddForce(_jump_force, ForceMode.VelocityChange);
    }
}
