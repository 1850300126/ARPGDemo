using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedAttackState : PlayerGroundedState
{

    protected bool find_target = false;
    public GroundedAttackState(PlayerMovementStateMachine player_movement_state_machine) : base(player_movement_state_machine)
    {

    }

    public override void OnEnter()
    {
        AddInputAction();

        Debug.Log("当前的状态" + this);
    }
    public override void OnExit()
    {
        RemoveInputAction();

        movement_state_machine.player.CloseDamageCollider();
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
    protected void PlayComboAnimationClip(string name, float fade_time = 0.05f)
    {
        movement_state_machine.player.animator.CrossFade(name, fade_time);
    }

    protected void PlayAnimationClipFinish(IState state)
    {
        movement_state_machine.ChangeState(state);
    }

    // 旋转到目标方向
    protected void RotateAttackableDirection()
    {
        if(movement_state_machine.reusable_data.target_trans != null)
        {
            Vector3 _self = new Vector3(movement_state_machine.player.transform.position.x, 0, movement_state_machine.player.transform.position.z);
            Vector3 _target = new Vector3(movement_state_machine.reusable_data.target_trans.position.x, 0, movement_state_machine.reusable_data.target_trans.position.z);
            Vector3 _direction = _target - _self;
            Quaternion _rotate = Quaternion.LookRotation(_direction);
            movement_state_machine.player.transform.localRotation = _rotate;
        }
        else
        {   
            // 让人物旋转至输入方向
            RotateInputDirection();
        }
        
    }
    // 旋转到输入方向
    protected void RotateInputDirection()
    {
        if (movement_state_machine.reusable_data.movement_input == Vector2.zero) return;

        Vector3 dashDirection = new Vector3(movement_state_machine.reusable_data.movement_input.x, 0, movement_state_machine.reusable_data.movement_input.y);

        Quaternion target_rot = Quaternion.Euler(0f, UpdateTargetRotation(dashDirection, true), 0f);

        movement_state_machine.player.transform.rotation = target_rot;
    }
    // 判断切片是否可打断的标签
    protected void JugdeClipAllowInterruption()
    {
        if (!movement_state_machine.player.animator.GetCurrentAnimatorStateInfo(0).IsTag("AllowInterruption")) return;

        if (movement_state_machine.reusable_data.movement_input == Vector2.zero)
        {
            return;
        }

        OnMove();
    }
    // 判断连招时间是否结束
    protected void JugdeComboFinish()
    {
        if(Time.time - movement_state_machine.reusable_data.last_attack_time > movement_state_machine.reusable_data.combo_time)
        {
            movement_state_machine.reusable_data.next_light_combo_index = 0;
        }
    }
    // 判断是否存在可攻击的物体
    protected void JugdeExistAttackableObject()
    {
        if(movement_state_machine.reusable_data.next_light_combo_index != 0) return;

        Collider[] colliders = Physics.OverlapSphere(movement_state_machine.player.transform.position, 6f , movement_state_machine.player.layer_data.AttackLayer);

        if(colliders.Length > 0 )
        {   
            
            Debug.Log("范围内有敌人");

            movement_state_machine.reusable_data.target_trans = colliders[0].transform;

            // 距离小于2不进行索敌位移
            if(Vector3.Distance(colliders[0].transform.position, movement_state_machine.player.transform.position) < 2) return;

            find_target = true;
        }
        else
        {
            movement_state_machine.reusable_data.target_trans = null;
        }
    }

}
