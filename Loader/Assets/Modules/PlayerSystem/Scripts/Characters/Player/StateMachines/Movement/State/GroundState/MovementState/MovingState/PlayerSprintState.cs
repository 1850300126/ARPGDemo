using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerMovingState
{
    public PlayerSprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {

    }

    private float startTime;

    private bool keepSprinting;
    private bool shouldResetSprintState;

    public override void OnEnter()
    {
        movement_state_machine.reusable_data.MovementSpeedModifier = grounded_data.SprintData.SpeedModifier;

        base.OnEnter();

        // StartAnimation(movement_state_machine.player.animation_data.SprintParameterHash);

        movement_state_machine.player.animator.CrossFade("Sprint", 0.1f);

        movement_state_machine.reusable_data.current_jump_force = airborne_data.JumpData.StrongForce;

        shouldResetSprintState = true;
        
        if (!movement_state_machine.reusable_data.ShouldSprint)
        {
            keepSprinting = false;
        }
        startTime = Time.time;
    }

    public override void OnExit()
    {
        base.OnExit();

        // StopAnimation(movement_state_machine.player.animation_data.SprintParameterHash);

        if (shouldResetSprintState)
        {
            keepSprinting = false;

            movement_state_machine.reusable_data.ShouldSprint = false;
        }
    }       
    public override void OnUpdate()
    {
        base.OnUpdate();

        SpiritReduce(Time.deltaTime * grounded_data.SprintData.sprint_reduce);
        
        if(movement_state_machine.player.player_data.self_data.spirit < 0)
        {
            StopSprinting(null);
        }

        if (keepSprinting)
        {
            return;
        }

        if (Time.time < startTime + grounded_data.SprintData.SprintToRunTime)
        {
            return;
        }    
        StopSprinting(null);
    }

    private void StopSprinting(object[] param)
    {
        if ( movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            OnStop(movement_state_machine.idle_state);

            return;
        }

        movement_state_machine.ChangeState(movement_state_machine.run_state);
    }

    protected override void AddInputAction()
    {
        base.AddInputAction();

        movement_state_machine.player.player_input.player_actions.Sprint.performed += OnSprintPerformed;
    }

    protected override void RemoveInputAction()
    {
        base.RemoveInputAction();

        movement_state_machine.player.player_input.player_actions.Sprint.performed -= OnSprintPerformed;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        keepSprinting = true;
        movement_state_machine.reusable_data.ShouldSprint = true;
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {   
        movement_state_machine.ChangeState(movement_state_machine.hard_stop_state);

        base.OnMovementCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        shouldResetSprintState = false;

        base.OnJumpStarted(context);
    } 
}
