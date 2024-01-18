using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using EasyUpdateDemoSDK;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HardAttackState : GroundedAttackState
{
    private int comb_index = 0;

    public HardAttackState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        movement_state_machine.player.animator.applyRootMotion = true;

        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;

        ResetVelocity();

        comb_index = 0;
        
        OnHardAttack();
    }
    public override void OnExit()
    {
        base.OnExit();

        movement_state_machine.player.animator.applyRootMotion = false;

        movement_state_machine.player.SkillController.InterruptSkill();
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
    }
    public override void OnAnimationExitEvent()
    { 
        PlayAnimationClipFinish(movement_state_machine.idle_state);
    }
    
    // protected override void OnLightAttackStarted(InputAction.CallbackContext context)
    // {
    //     if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, movement_state_machine.player.current_combo_config.hard_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

    //     movement_state_machine.ChangeState(movement_state_machine.light_attack_state);
    // }    
    // protected override void OnHardAttackStarted(InputAction.CallbackContext context)
    // {   
    //     Debug.Log(comb_index);

    //     if(comb_index == 1) return;

    //     if(AttackForwardShake(ref movement_state_machine.reusable_data.last_attack_time, movement_state_machine.player.current_combo_config.hard_attack_configs[movement_state_machine.reusable_data.next_light_combo_index - 1].relaese_time)) return;

    //     OnHardAttack();
    // }  
    protected void OnHardAttack()
    {
        HardAttack();
    }
    protected void HardAttack()
    {
        RotateAttackableDirection();
         // 播放动画切片
        movement_state_machine.player.SkillController.PlaySkill(movement_state_machine.player.currentWeaponAnimationConfigs.hard_attack_configs[movement_state_machine.reusable_data.current_combo_index - 1], null);
    }
}
