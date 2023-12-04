using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirborneState
{
    private Vector3 playerPositionOnEnter;
    public float fall_speed_limit = 5f;

    public PlayerFallState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
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

        // StartAnimation(movement_state_machine.player.animation_data.FallParameterHash);
        movement_state_machine.player.animator.CrossFade("Fall", 0.1f);

        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;

        playerPositionOnEnter = movement_state_machine.player.transform.position;

        ResetVerticalVelocity();
    }

    public override void OnExit()
    {
        base.OnExit();

        // StopAnimation(movement_state_machine.player.animation_data.FallParameterHash);
    }
    public override void OnUpdate() 
    {

    }        
    public override void OnFixUpdate()
    {
        base.OnFixUpdate();

        LimitVerticalVelocity();    
    }
    #endregion
    private void LimitVerticalVelocity()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

        if (playerVerticalVelocity.y >= -fall_speed_limit)
        {
            return;
        }

        Vector3 limitedVelocityForce = new Vector3(0f, -fall_speed_limit - playerVerticalVelocity.y, 0f);

        movement_state_machine.player.player_rb.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
    }        
    protected override void OnContactWithGround(Collider collider)
    {
        float fallDistance = playerPositionOnEnter.y - movement_state_machine.player.transform.position.y;

        if (fallDistance < 3f)
        {
            movement_state_machine.ChangeState(movement_state_machine.light_land_state);

            return;
        }
        movement_state_machine.ChangeState(movement_state_machine.light_land_state);

    }
}
