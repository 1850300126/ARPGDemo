using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{   
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        enemy_state_machine.reusable_data.origin_pos = enemy_state_machine.enemy.transform.position;
    }

    public float enter_patrol_state_time;

    public override void OnEnter()
    {
        base.OnEnter();

        enemy_state_machine.enemy.animator.CrossFade("Idle", 0.1f);

        enter_patrol_state_time = GetEnterPatrolTime();

        enemy_state_machine.enemy.agent.velocity = Vector3.zero;

        enemy_state_machine.enemy.agent.stoppingDistance = 0;

        enemy_state_machine.enemy.agent.speed = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        JudgeEnemyPatrol(ref enter_patrol_state_time);
    }

    public float GetEnterPatrolTime(float min = 3, float max = 5)
    {
        return Random.Range(min, max);
    }

}
