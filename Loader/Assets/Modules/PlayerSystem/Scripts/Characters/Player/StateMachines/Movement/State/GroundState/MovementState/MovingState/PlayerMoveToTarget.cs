using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Debug.Log(target_trans.position);
        Debug.Log(target_trans.gameObject.name);
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();

        Vector3 lerp_pos = Vector3.Lerp(movement_state_machine.player.player_rb.transform.position, target_trans.position, Time.fixedDeltaTime * 5f);
        
        movement_state_machine.player.player_rb.transform.position = lerp_pos;

        if(Vector3.Distance(movement_state_machine.player.player_rb.transform.position, movement_state_machine.player.transform.position) < 0.5f)
        {
            action?.Invoke();
        }
    }
    public override void OnExit()
    {

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
