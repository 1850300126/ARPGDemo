using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyStateBase
{   
    public EnemyPatrolState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        enemy_state_machine.enemy.animator.CrossFade("Walk", 0.1f);
        
        enemy_state_machine.enemy.agent.enabled = true;
        // 获取即将要到达的目标点
        enemy_state_machine.reusable_data.agent_target_pos = GetPatrolTargetPos();
        
        enemy_state_machine.enemy.agent.stoppingDistance = 0;

        enemy_state_machine.enemy.agent.speed = 2;

        enemy_state_machine.enemy.agent.SetDestination(enemy_state_machine.reusable_data.agent_target_pos);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        AgentMoveTargetPos();
    }

    protected Vector3 GetPatrolTargetPos()
    {   
        enemy_state_machine.reusable_data.agent_target_pos = enemy_state_machine.reusable_data.origin_pos;
        // 获取最大纵向、横向距离
        float min_x = -5;
        float max_x = 5;

        float min_z = -5;
        float max_z = 5;
        // 获取与远点不能超过的最大距离
        float max_distance_origin = 10;

        Vector3 current_pos = new Vector3(GetEnemyCurrentPos().x, 0, GetEnemyCurrentPos().z);

        if(Vector3.Distance(enemy_state_machine.reusable_data.origin_pos, current_pos) > max_distance_origin)
        {
            return enemy_state_machine.reusable_data.agent_target_pos;
        }
        else
        {
                
            float pos_x = Random.Range(min_x, max_x);

            float pos_z = Random.Range(min_z, max_z);

            enemy_state_machine.reusable_data.agent_target_pos = new Vector3(pos_x, 0, pos_z);

            return enemy_state_machine.reusable_data.agent_target_pos;
        }
    }

    protected void AgentMoveTargetPos()
    {    
        if(!enemy_state_machine.enemy.agent.pathPending && enemy_state_machine.enemy.agent.remainingDistance > 0.1f) return;

        enemy_state_machine.ChangeState(enemy_state_machine.idle_state);
    }

    protected void RotateTarget()
    {
        Vector3 target = enemy_state_machine.reusable_data.agent_target_pos - enemy_state_machine.enemy.enemy_rb.transform.position;

        Vector3 direction = (target - Vector3.forward).normalized;

        Rotate(direction, () => {
            
        });
    }
}
