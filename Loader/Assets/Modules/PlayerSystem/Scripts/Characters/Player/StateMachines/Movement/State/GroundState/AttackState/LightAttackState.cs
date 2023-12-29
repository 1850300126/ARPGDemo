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

        JugdeExistAttackableObject();
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

        if(!find_target) return;
        
        Vector3 lerp_pos = Vector3.Lerp(movement_state_machine.player.player_rb.transform.position, movement_state_machine.reusable_data.target_trans.position, Time.fixedDeltaTime * 3f);

        movement_state_machine.player.player_rb.transform.position = lerp_pos;

        if(Vector3.Distance(movement_state_machine.player.player_rb.transform.position, movement_state_machine.reusable_data.target_trans.position) < 2f)
        {
            find_target = false;
        }
 
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnAnimationEnterEvent()
    {
       
    }
    public override void OnAnimationExitEvent()
    {  
        PlayAnimationClipFinish(movement_state_machine.light_attack_finish_state);
    }

    protected override void OnLightAttackStarted(InputAction.CallbackContext context)
    {
        if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, light_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

        movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
    }     
    protected override void OnHardAttackStarted(InputAction.CallbackContext context)
    {
        if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, light_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

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
        RotateAttackableDirection();
        // 播放动画切片
        PlayComboAnimationClip(animation_name);
    }
}