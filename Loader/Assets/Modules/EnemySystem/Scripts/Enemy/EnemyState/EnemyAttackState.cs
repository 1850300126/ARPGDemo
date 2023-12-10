using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    public EnemyAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        enemy_state_machine.enemy.animator.CrossFade("Attack_01", 0.1f);
    }

    
}
