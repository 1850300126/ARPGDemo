using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedAttackState : PlayerGroundedState
{
    public GroundedAttackState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(movement_state_machine.player.animation_data.AttackParameterHash);
    }
    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(movement_state_machine.player.animation_data.AttackParameterHash);
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

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

    /// <summary>
    /// 判断是否在前摇阶段（动作必须播的时间内）
    /// </summary>
    /// <param name="last_attack_time">本次攻击开始的时间</param>
    /// <param name="attack_animation_foward_shake">本次攻击的前摇时间</param>
    /// <returns></returns>
    protected bool AttackForwardShake(ref float last_attack_time, float attack_animation_foward_shake)
    {        
        if(Time.time < last_attack_time + attack_animation_foward_shake)
        {   
            return true;
        }
        else
        {
            last_attack_time = Time.time;
            return false;
        }
    }
    protected void PlayComboAnimationClip(string name, float fade_time = 0.1f)
    {   
        movement_state_machine.player.animator.CrossFade(name, fade_time);
    }

    protected void PlayAnimationClipFinish(IState state)
    {
        movement_state_machine.ChangeState(state);
    }

    protected void RotatePlayer()
    {   
        if(movement_state_machine.reusable_data.movement_input == Vector2.zero) return;

        Vector3 dashDirection = new Vector3(movement_state_machine.reusable_data.movement_input.x, 0, movement_state_machine.reusable_data.movement_input.y);
        
        Quaternion target_rot = Quaternion.Euler(0f, UpdateTargetRotation(dashDirection, true), 0f);

        movement_state_machine.player.transform.rotation = target_rot;
    }

    protected void JugdeClipAllowInterruption()
    {
        if(!movement_state_machine.player.animator.GetCurrentAnimatorStateInfo(0).IsTag("AllowInterruption")) return;

        if(movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            return;
        }
        OnMove();

    }
}
