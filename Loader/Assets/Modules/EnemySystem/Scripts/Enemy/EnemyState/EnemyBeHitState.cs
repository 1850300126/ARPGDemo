using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeHitState : EnemyStateBase
{
    public EnemyBeHitState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        // enemy_state_machine.enemy.PlayAnimation("GetHit");
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnAnimationExitEvent()
    {
        enemy_state_machine.ChangeState(enemy_state_machine.idle_state);
    }
}
