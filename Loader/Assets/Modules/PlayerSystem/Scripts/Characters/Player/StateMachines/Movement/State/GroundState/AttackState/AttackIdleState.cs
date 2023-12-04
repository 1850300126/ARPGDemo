using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackIdleState : GroundedAttackState
{   
    public AttackIdleState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();

        // StartAnimation(movement_state_machine.player.animation_data.AttackIdleParameterHash);
        movement_state_machine.player.animator.CrossFade("FightIdle", 0.1f);
        // 使用根运动
        movement_state_machine.player.animator.applyRootMotion = true;

        movement_state_machine.reusable_data.MovementSpeedModifier = 0;

        ResetVelocity();
    }
    public override void OnExit()
    {
        base.OnExit();

        // StopAnimation(movement_state_machine.player.animation_data.AttackIdleParameterHash);

        movement_state_machine.player.animator.applyRootMotion = false;
    }
    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
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
    public override void OnAnimationTransitionEvent()
    {
        movement_state_machine.ChangeState(movement_state_machine.idle_state);
    }
}
