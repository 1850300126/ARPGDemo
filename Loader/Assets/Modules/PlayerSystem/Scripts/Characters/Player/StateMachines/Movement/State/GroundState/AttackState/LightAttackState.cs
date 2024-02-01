using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class LightAttackState : GroundedAttackState
{
    public LightAttackState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        // 屏蔽移动
        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;
        // 重置速度
        ResetVelocity();
        // 判断是否存在可攻击的物体
        JugdeExistAttackableObject();
        // 进行一次攻击
        OnLightAttack();
    }
    public override void OnExit()
    {
        base.OnExit();

        // movement_state_machine.player.SkillController.InterruptSkill();
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();

        if(movement_state_machine.reusable_data.target_trans != null)
        {
            Vector3 _self = new Vector3(movement_state_machine.player.transform.position.x, 0, movement_state_machine.player.transform.position.z);
            Vector3 _target = new Vector3(movement_state_machine.reusable_data.target_trans.position.x, 0, movement_state_machine.reusable_data.target_trans.position.z);
            Vector3 _direction = _target - _self;
            Quaternion targetRotation = Quaternion.LookRotation(_direction, Vector3.up);
            movement_state_machine.player.transform.rotation = targetRotation;
        }


        if(find_target)
        {
            Vector3 lerp_pos = Vector3.Lerp(movement_state_machine.player.player_rb.transform.position, movement_state_machine.reusable_data.target_trans.position, Time.fixedDeltaTime * 3f);
            movement_state_machine.player.player_rb.transform.position = lerp_pos;

            if(Vector3.Distance(movement_state_machine.player.player_rb.transform.position, movement_state_machine.reusable_data.target_trans.position) < 2f)
            {
                find_target = false;
            }
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
        PlayAnimationClipFinish(movement_state_machine.idle_state);
    }

    protected override void OnLightAttackStarted(InputAction.CallbackContext context)
    {     
        // if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, movement_state_machine.player.currentWeaponAnimationConfigs.light_attack_configs[movement_state_machine.reusable_data.current_combo_index - 1].AfterShaking)) return;

        RotateAttackableDirection();

        movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
    }     
    protected void OnLightAttack()
    {   
        if(movement_state_machine.reusable_data.current_combo_index < movement_state_machine.player.currentSkillConfig.timelines.Count)
        {   
            LightAttack();
            movement_state_machine.reusable_data.current_combo_index += 1;
        }
        else
        {
            movement_state_machine.reusable_data.current_combo_index = 0;

            movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
        }  
        // if(movement_state_machine.reusable_data.current_combo_index < movement_state_machine.player.currentWeaponAnimationConfigs.light_attack_configs.Count)
        // {   
        //     LightAttack();
        //     movement_state_machine.reusable_data.current_combo_index += 1;
        // }
        // else
        // {
        //     movement_state_machine.reusable_data.current_combo_index = 0;

        //     movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
        // }
    }

    protected void LightAttack()
    {
        movement_state_machine.reusable_data.last_attack_time = Time.time;
        // 播放动画切片
        // movement_state_machine.player.SkillController.PlaySkill(
        //     movement_state_machine.player.currentWeaponAnimationConfigs.light_attack_configs[movement_state_machine.reusable_data.current_combo_index], null, OnRootMotion);
        Debug.Log(movement_state_machine.reusable_data.current_combo_index);
        movement_state_machine.player.AnimationController.PlayTimeline
        (movement_state_machine.player.currentSkillConfig.timelines[movement_state_machine.reusable_data.current_combo_index],
        () => movement_state_machine.ChangeState(movement_state_machine.idle_state));
    }
}