using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveToTarget : PlayerMovingState
{   
    public Transform target_trans;
    public IState next_state;
    public float move_time = 0.5f;
    public float move_time_count;
    public PlayerMoveToTarget(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        AddInputAction();

        move_time_count = move_time;

        movement_state_machine.player.transform.LookAt(new Vector3(target_trans.position.x, 0, target_trans.position.z));

        movement_state_machine.ChangeState(next_state);
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixUpdate()
    {
        base.OnFixUpdate();
    }
}
