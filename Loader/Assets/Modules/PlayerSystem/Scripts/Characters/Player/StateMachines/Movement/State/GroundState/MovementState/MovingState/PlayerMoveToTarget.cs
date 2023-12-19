using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = System.Action;

public class PlayerMoveToTarget : PlayerMovingState
{   
    public Transform target_trans;
    public IState next_state;
    public float move_time = 0.5f;
    public float move_time_count;
    public Action action;
    public PlayerMoveToTarget(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        AddInputAction();

        move_time_count = move_time;

        Vector3 target_pos = new Vector3(target_trans.position.x, 0, target_trans.position.z);

        movement_state_machine.player.transform.LookAt(target_pos);
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();

        Vector3 lerp_pos = Vector3.Lerp(movement_state_machine.player.player_rb.transform.position, target_trans.position, Time.fixedDeltaTime * 10f);
        
        Debug.Log(Vector3.Distance(movement_state_machine.player.player_rb.transform.position, target_trans.position));

        movement_state_machine.player.player_rb.transform.position = lerp_pos;

        if(Vector3.Distance(movement_state_machine.player.player_rb.transform.position, target_trans.position) < 2f)
        {
            action?.Invoke();
        }
    }
    public override void OnHandleInput()
    {
        
    }
    public override void OnExit()
    {
        RemoveInputAction();
    }
    protected override void AddInputAction()
    {
        movement_state_machine.player.player_input.player_actions.Jump.started += OnJumpStarted;

        movement_state_machine.player.player_input.player_actions.Dodge.started += OnDodgeStarted;
    }

    protected override void RemoveInputAction()
    {
        movement_state_machine.player.player_input.player_actions.Jump.started -= OnJumpStarted;

        movement_state_machine.player.player_input.player_actions.Dodge.started -= OnDodgeStarted;
    }

}
