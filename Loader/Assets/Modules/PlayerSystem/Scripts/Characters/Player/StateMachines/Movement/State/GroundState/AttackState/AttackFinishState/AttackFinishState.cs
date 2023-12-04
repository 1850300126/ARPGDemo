using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFinishState : GroundedAttackState
{
    public AttackFinishState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    
    public override void OnEnter()
    {
        base.OnEnter();

        // StartAnimation(movement_state_machine.player.animation_data.AttackFinishParameterHash);
        // 屏蔽移动
        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;
        movement_state_machine.player.animator.applyRootMotion = true;
        // 重置速度
        ResetVelocity();
    }
    public override void OnExit()
    {
        base.OnExit();
        movement_state_machine.player.animator.applyRootMotion = false;
        // StopAnimation(movement_state_machine.player.animation_data.AttackFinishParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            return;
        }

        OnMove();
    }    
    public override void OnAnimationExitEvent()
    { 
        PlayAnimationClipFinish(movement_state_machine.attack_idle_state);
    }

}
