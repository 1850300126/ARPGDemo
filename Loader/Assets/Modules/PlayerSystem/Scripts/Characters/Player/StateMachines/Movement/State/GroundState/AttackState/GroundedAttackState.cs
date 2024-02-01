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

        Debug.Log("��ǰ��״̬" + this);
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
    /// �ж�??����ǰҡ��???����������??��ʱ����??
    /// </summary>
    /// <param name="last_attack_time">??�ι�����ʼ��ʱ��</param>
    /// <param name="attack_animation_foward_shake">??�ι�����ǰҡʱ��</param>
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

    // ��ת��Ŀ�귽??
    protected void RotateAttackableDirection()
    {
        if(movement_state_machine.reusable_data.target_trans == null)
        {
            // ��������ת�����뷽��
            RotateInputDirection();
        }
        
    }
    // ��ת�����뷽??
    protected void RotateInputDirection()
    {
        if (movement_state_machine.reusable_data.movement_input == Vector2.zero) return;

        Vector3 dashDirection = new Vector3(movement_state_machine.reusable_data.movement_input.x, 0, movement_state_machine.reusable_data.movement_input.y);

        Quaternion target_rot = Quaternion.Euler(0f, UpdateTargetRotation(dashDirection, true), 0f);

        movement_state_machine.player.transform.rotation = target_rot;
    }
    // �ж��Ƿ�������ʱ����
    protected void JugdeComboFinish()
    {

    }
    // �ж��Ƿ�����ƶ�
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
    // �ж��Ƿ���ڿɹ���������
    protected void JugdeExistAttackableObject()
    {
        Collider[] colliders = Physics.OverlapSphere(movement_state_machine.player.transform.position, 6f , movement_state_machine.player.layer_data.AttackLayer);

        if(colliders.Length > 0 )
        {   
            movement_state_machine.reusable_data.target_trans = colliders[0].transform;

            // ����С��2����������
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
