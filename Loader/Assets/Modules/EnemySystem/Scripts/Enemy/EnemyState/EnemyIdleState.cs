using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{   
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        
    }

    public float enter_patrol_state_time;

    public override void OnEnter()
    {
        base.OnEnter();

        enemy_state_machine.enemy.animator.CrossFade("Idle", 0.1f);

        enter_patrol_state_time = GetEnterPatrolTime();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        JudgeEnemyPatrol(ref enter_patrol_state_time);
    }

    public float GetEnterPatrolTime(float min = 1, float max = 5)
    {
        return Random.Range(min, max);
    }

}
