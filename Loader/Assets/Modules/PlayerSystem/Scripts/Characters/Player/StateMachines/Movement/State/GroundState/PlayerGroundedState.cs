using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();

        UpdateShouldSprintState();

        ResetAttackIndex();
    }        
    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
        Float();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    protected override void AddInputAction()
    {
        base.AddInputAction();

        movement_state_machine.player.player_input.player_actions.Jump.started += OnJumpStarted;

        movement_state_machine.player.player_input.player_actions.Movement.canceled += OnMovementCanceled;
    }

    protected override void RemoveInputAction()
    {
        base.RemoveInputAction();

        movement_state_machine.player.player_input.player_actions.Jump.started -= OnJumpStarted;

        movement_state_machine.player.player_input.player_actions.Movement.canceled -= OnMovementCanceled;
    }    
    protected override void OnMovementPerformed(InputAction.CallbackContext context)
    {
        base.OnMovementPerformed(context);

        UpdateTargetRotation(GetMovementInputDirection());
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        movement_state_machine.ChangeState(movement_state_machine.jump_state);
    }          
    protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
    {   
        // if(movement_state_machine.player.player_data.self_data.spirit < 20)
        // {
        //     return;
        // }

        // SpiritReduce(grounded_data.DodgeData.dodge_reduce);

        movement_state_machine.ChangeState(movement_state_machine.dodge_state);
    }     
    protected virtual void OnLightAttackStarted(InputAction.CallbackContext context)
    {  
        movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
    }     
    protected virtual void OnHardAttackStarted(InputAction.CallbackContext context)
    {
       // movement_state_machine.ChangeState(movement_state_machine.hard_attack_state);
    } 
    protected virtual void OnMove()
    {            
        if (movement_state_machine.reusable_data.ShouldSprint)
        {
            movement_state_machine.ChangeState(movement_state_machine.sprint_state);

            return;
        }
        movement_state_machine.ChangeState(movement_state_machine.run_state);
    }        
    protected override void OnContactWithGroundExited(Collider collider) 
    {

        if (IsThereGroundUnderneath())
        {
            return;
        }

        Vector3 capsuleColliderCenterInWorldSpace = movement_state_machine.player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - movement_state_machine.player.ResizableCapsuleCollider.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

        if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, grounded_data.GroundToFallRayDistance, movement_state_machine.player.layer_data.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFall();
        }
    }        
    private bool IsThereGroundUnderneath()
    {
        PlayerTriggerColliderData triggerColliderData = movement_state_machine.player.ResizableCapsuleCollider.TriggerColliderData;

        Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;

        Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.GroundCheckColliderVerticalExtents, triggerColliderData.GroundCheckCollider.transform.rotation, movement_state_machine.player.layer_data.GroundLayer, QueryTriggerInteraction.Ignore);

        return overlappedGroundColliders.Length > 0;
    }
    protected virtual void OnFall()
    {
        movement_state_machine.ChangeState(movement_state_machine.fall_state);
    }
    protected void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = movement_state_machine.player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, movement_state_machine.player.ResizableCapsuleCollider.SlopeData.FloatRayDistance, movement_state_machine.player.layer_data.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = movement_state_machine.player.ResizableCapsuleCollider.CapsuleColliderData.ColliderCenterInLocalSpace.y * movement_state_machine.player.transform.localScale.y - hit.distance;

            if (distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = distanceToFloatingPoint * movement_state_machine.player.ResizableCapsuleCollider.SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            movement_state_machine.player.player_rb.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }        
    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = grounded_data.SlopeSpeedAngles.Evaluate(angle);

        if (movement_state_machine.reusable_data.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
        {
            movement_state_machine.reusable_data.MovementOnSlopesSpeedModifier = slopeSpeedModifier;
        }

        return slopeSpeedModifier;
    }        
    private void UpdateShouldSprintState()
    {
        if (!movement_state_machine.reusable_data.ShouldSprint)
        {
            return;
        }

        if (movement_state_machine.reusable_data.movement_input != Vector2.zero)
        {
            return;
        }

        movement_state_machine.reusable_data.ShouldSprint = false;
    }
}
