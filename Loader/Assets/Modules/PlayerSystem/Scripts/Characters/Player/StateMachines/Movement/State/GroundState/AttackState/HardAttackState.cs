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

        movement_state_machine.reusable_data.MovementSpeedModifier = 0f;

        ResetVelocity();

        comb_index = 0;
        
        OnHardAttack();
    }
    public override void OnExit()
    {
        base.OnExit();

        // movement_state_machine.player.SkillController.InterruptSkill();
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
    protected void OnHardAttack()
    {
        HardAttack();
    }
    protected void HardAttack()
    {
        RotateAttackableDirection();
         // �?放动画切�?
        // movement_state_machine.player.SkillController.PlaySkill(movement_state_machine.player.currentWeaponAnimationConfigs.hard_attack_configs[movement_state_machine.reusable_data.current_combo_index - 1], null, OnRootMotion);
    }
}
