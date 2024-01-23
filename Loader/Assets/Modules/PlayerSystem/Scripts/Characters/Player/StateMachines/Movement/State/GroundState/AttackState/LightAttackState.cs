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
        JugdeExistAttackableObject();
        // 进行一次攻击
        OnLightAttack();
    }
    public override void OnExit()
    {
        base.OnExit();

        movement_state_machine.player.SkillController.InterruptSkill();
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
        PlayAnimationClipFinish(movement_state_machine.idle_state);
    }
    protected override void OnHardAttackStarted(InputAction.CallbackContext context)
    {
        movement_state_machine.ChangeState(movement_state_machine.hard_attack_state);
    } 

    protected void OnLightAttack()
    {   
        
        if(movement_state_machine.reusable_data.current_combo_index < movement_state_machine.player.currentWeaponAnimationConfigs.light_attack_configs.Count)
        {
            LightAttack();
            movement_state_machine.reusable_data.current_combo_index += 1;
        }
        else
        {
            movement_state_machine.reusable_data.current_combo_index = 0;

            movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
        }
    }

    protected void LightAttack()
    {
        RotateAttackableDirection();
        // 播放动画切片
        movement_state_machine.player.SkillController.PlaySkill(movement_state_machine.player.currentWeaponAnimationConfigs.light_attack_configs[movement_state_machine.reusable_data.current_combo_index], null, OnRootMotion);
    }
}