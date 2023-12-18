using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightAttackState : GroundedAttackState
{
    private List<LightAttackConfig> light_attack_configs;
    public LightAttackState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();

        // 使用根运动
        movement_state_machine.player.animator.applyRootMotion = true;
        // 屏蔽移动
        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;
        // 重置速度
        ResetVelocity();

        light_attack_configs = movement_state_machine.player.current_combo_config.light_attack_configs;

        // 进行一次攻击
        OnLightAttack();
    }
    public override void OnExit()
    {
        base.OnExit();

        movement_state_machine.player.animator.applyRootMotion = false;
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnAnimationEnterEvent()
    {
        // 播放特效
        APISystem.instance.CallAPI("VFX_system", "play_particle_from_config", new object[]{light_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].particle_configs[0], movement_state_machine.player.transform});
    }
    public override void OnAnimationExitEvent()
    {  
        PlayAnimationClipFinish(movement_state_machine.light_attack_finish_state);
    }
    protected override void OnLightAttackStarted(InputAction.CallbackContext context)
    {
        if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, light_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

        MoveToEnemyAttack(movement_state_machine.light_attack_state);

        OnLightAttack();
    }     
    protected override void OnHardAttackStarted(InputAction.CallbackContext context)
    {
        if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, light_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

        MoveToEnemyAttack(movement_state_machine.hard_attack_state);

        movement_state_machine.ChangeState(movement_state_machine.hard_attack_state);
    } 

    protected void OnLightAttack()
    {   
        movement_state_machine.reusable_data.last_attack_time = Time.time;

        int current_light_attack_index = movement_state_machine.reusable_data.next_light_combo_index;

        if(current_light_attack_index < light_attack_configs.Count)
        {
            LightAttack(light_attack_configs[current_light_attack_index].light_attack_clip_name);

            movement_state_machine.reusable_data.next_light_combo_index += 1;
        }
        else
        {
            movement_state_machine.reusable_data.next_light_combo_index = 0;

            movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
        }
    }

    protected void LightAttack(string animation_name)
    {
        // 让人物旋转至输入方向
        RotatePlayer();
        // 播放动画切片
        PlayComboAnimationClip(animation_name);
    }
}