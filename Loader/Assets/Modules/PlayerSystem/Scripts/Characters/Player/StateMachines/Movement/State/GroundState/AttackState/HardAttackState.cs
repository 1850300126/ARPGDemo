using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HardAttackState : GroundedAttackState
{
    private float last_attack_time;
    private int comb_index;

    public HardAttackState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(movement_state_machine.player.animation_data.AttackHardParameterHash);
        // 使用根运动
        movement_state_machine.player.animator.applyRootMotion = true;
        // 屏蔽移动
        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;
        // 重置速度
        ResetVelocity();
        // 执行攻击
        OnHardAttack();
    }
    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(movement_state_machine.player.animation_data.AttackHardParameterHash);

        movement_state_machine.player.animator.applyRootMotion = false;
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        // 是否过渡到允许移动的片段
        JugdeClipAllowInterruption();
    }
    public override void OnAnimationEnterEvent()
    {
        APISystem.instance.CallAPI("VFX_system", "play_particle_from_config", 
        new object[]{movement_state_machine.player.current_combo_config.hard_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].particle_configs[0],
        movement_state_machine.player.transform});
    }
    public override void OnAnimationExitEvent()
    { 
        PlayAnimationClipFinish(movement_state_machine.attack_finish_state);
    }
    
    protected override void OnLightAttackStarted(InputAction.CallbackContext context)
    {
        if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, movement_state_machine.player.current_combo_config.hard_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

        movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
    }    
    protected override void OnHardAttackStarted(InputAction.CallbackContext context)
    {
        if(comb_index == 1) return;

        if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, movement_state_machine.player.current_combo_config.hard_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

        OnHardAttack();
    }  
    protected void OnHardAttack()
    {
        movement_state_machine.reusable_data.last_attack_time = Time.time;
        // 判断该次轻攻击是否对应的有重攻击
        Debug.Log(movement_state_machine.reusable_data.next_light_combo_index - 1);
        if(movement_state_machine.player.current_combo_config.hard_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1] == null) return;

        comb_index ++ ;

        HardAttack(movement_state_machine.player.current_combo_config.hard_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].hard_attack_clip_name);
    }
    protected void HardAttack(string animation_name)
    {
        // 让人物旋转至输入方向
        RotatePlayer();
        // 播放动画切片
        PlayComboAnimationClip(animation_name);
        

    }
}
