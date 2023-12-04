using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerMovingState
{
    public PlayerRunState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
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

        // StartAnimation(movement_state_machine.player.animation_data.RunParameterHash);

        movement_state_machine.player.animator.CrossFade("Run", 0.1f);

        movement_state_machine.reusable_data.current_jump_force = airborne_data.JumpData.WeakForce;

            // movement_state_machine.reusable_data.MovementSpeedModifier = grounded_data.RunData.SpeedModifier;

        movement_state_machine.reusable_data.MovementSpeedModifier = 1;
    }

    public override void OnUpdate()
    {
        if(movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            OnStop(movement_state_machine.light_stop_state);
        }

        SpiritAdd(Time.deltaTime * 5);
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }

    public override void OnExit()
    {   
        base.OnExit();

        // StopAnimation(movement_state_machine.player.animation_data.RunParameterHash);
    }
    #endregion
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
}
