using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{   
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        enemy_state_machine.reusable_data.origin_pos = enemy_state_machine.enemy.transform.position;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // enemy_state_machine.enemy.PlayAnimation("Idle");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

}
