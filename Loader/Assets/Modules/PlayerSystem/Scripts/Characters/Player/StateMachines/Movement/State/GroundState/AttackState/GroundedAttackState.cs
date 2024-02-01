using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedAttackState : PlayerGroundedState
{

    protected bool find_target = false;
    protected float moveTime;
    public GroundedAttackState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        AddInputAction();

        moveTime = 0;

        Debug.Log("当前的状态" + this);
    }
    public override void OnExit()
    {
        RemoveInputAction();

        MsgSystem.instance.SendMsg("AttackExit", null);
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        JugdeMove();

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
    /// 判断??否在前摇阶???（动作必须??的时间内??
    /// </summary>
    /// <param name="last_attack_time">??次攻击开始的时间</param>
    /// <param name="attack_animation_foward_shake">??次攻击的前摇时间</param>
    /// <returns></returns>
    protected bool AttackForwardShake(ref float last_attack_time, float attack_animation_foward_shake)
    {
        if (Time.time < last_attack_time + attack_animation_foward_shake)
        {
            return true;
        }
        else
        {
            last_attack_time = Time.time;
            return false;
        }
    }

    protected void PlayAnimationClipFinish(IState state)
    {
        movement_state_machine.ChangeState(state);
    }

    // 旋转到目标方??
    protected void RotateAttackableDirection()
    {
        if(movement_state_machine.reusable_data.target_trans == null)
        {
            // 让人物旋转到输入方向
            RotateInputDirection();
        }
        
    }
    // 旋转到输入方??
    protected void RotateInputDirection()
    {
        if (movement_state_machine.reusable_data.movement_input == Vector2.zero) return;

        Vector3 dashDirection = new Vector3(movement_state_machine.reusable_data.movement_input.x, 0, movement_state_machine.reusable_data.movement_input.y);

        Quaternion target_rot = Quaternion.Euler(0f, UpdateTargetRotation(dashDirection, true), 0f);

        movement_state_machine.player.transform.rotation = target_rot;
    }
    // 判断是否在连招时间内
    protected void JugdeComboFinish()
    {

    }
    // 判断是否可以移动
    protected void JugdeMove()
    {       
        moveTime += Time.deltaTime;
        // if(moveTime > movement_state_machine.player.currentWeaponAnimationConfigs.light_attack_configs[movement_state_machine.reusable_data.current_combo_index - 1].AfterShaking + 0.2f)
        // {
        //     if (movement_state_machine.reusable_data.movement_input == Vector2.zero)
        //     {
        //         return;
        //     }

        //     OnMove();
        // }

    }   
    // 判断是否存在可攻击的物体
    protected void JugdeExistAttackableObject()
    {
        Collider[] colliders = Physics.OverlapSphere(movement_state_machine.player.transform.position, 6f , movement_state_machine.player.layer_data.AttackLayer);

        if(colliders.Length > 0 )
        {   
            movement_state_machine.reusable_data.target_trans = colliders[0].transform;

            // 距离小于2不进行索敌
            if(Vector3.Distance(colliders[0].transform.position, movement_state_machine.player.transform.position) < 2) return;

            find_target = true;
        }
        else
        {
            movement_state_machine.reusable_data.target_trans = null;
        }
    }
    protected void OnRootMotion(Vector3 deltaPosition, Quaternion deltaRotation)
    {   
        movement_state_machine.player.player_rb.position += deltaPosition;
        movement_state_machine.player.animator.gameObject.transform.rotation *= deltaRotation;
    }
}
