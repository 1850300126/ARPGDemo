using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyStateBase
{   
    public Vector3 target_pos;
    public EnemyPatrolState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        enemy_state_machine.enemy.animator.CrossFade("Walk", 0.1f);
        // 获取即将要到达的目标点
        target_pos = GetPatrolTargetPos();
        // 设置目标点
        enemy_state_machine.enemy.agent.SetDestination(target_pos);
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        AgentMoveTargetPos();
    }

    public Vector3 GetPatrolTargetPos()
    {   
        target_pos = origin_pos;
        // 获取最大纵向、横向距离
        float min_x = 3;
        float max_x = 6;

        float min_z = 3;
        float max_z = 6;
        // 获取与远点不能超过的最大距离
        float max_distance_origin = 10;

        Vector3 current_pos = new Vector3(GetEnemyCurrentPos().x, 0, GetEnemyCurrentPos().z);

        if(Vector3.Distance(origin_pos, current_pos) > max_distance_origin)
        {
            return target_pos;
        }
        else
        {
                
            float pos_x = Random.Range(min_x, max_x);

            float pos_z = Random.Range(min_z, max_z);

            target_pos = new Vector3(pos_x, 0, pos_z);

            return target_pos;
        }
    }

    public void AgentMoveTargetPos()
    {    
        if(enemy_state_machine.enemy.agent.remainingDistance > 0.1f) return;

        enemy_state_machine.ChangeState(enemy_state_machine.idle_state);
    }
}
