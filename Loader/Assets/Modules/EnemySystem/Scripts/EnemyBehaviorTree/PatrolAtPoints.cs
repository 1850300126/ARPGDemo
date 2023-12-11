using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.Burst.Intrinsics;
using UnityEngine;
using DG.Tweening;
public class PatrolAtPoints : EnemyConditionBase
{   
    public Vector3 target_point;
    // 获取与远点不能超过的最大距离
    public float max_distance_origin = 10;    
    private float wait_time = 2f;
    private float wait_counter = 0f;
    private bool waiting = true;
    public float speed = 5;

    public override TaskStatus OnUpdate()
    {
        if(enemy == null)
        {
            enemy = self_transform.Value.GetComponent<Enemy>();
        }
        return Patrol();
    }

    public TaskStatus Patrol()
    {
        if(waiting)
        {
            wait_counter += Time.deltaTime;
            if(wait_counter >= wait_time)
            {
                GetPatrolTargetPos();
                AgentMoveToTarget(target_point, target_point, 0.2f, 2, 0f);
                PlayAnimation("Walk");
                waiting = false;
            }
        }
        else
        {   
            if(!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.1f)
            {   
                PlayAnimation("Idle");
                wait_counter = 0f;
                waiting = true;
            }

        }
        return TaskStatus.Running;
    }
    protected void GetPatrolTargetPos()
    {   
        
        // 获取最大纵向、横向距离
        float min_x = -5;
        float max_x = 5;

        float min_z = -5;
        float max_z = 5;

        Vector3 current_pos = new Vector3(self_transform.Value.position.x, 0, self_transform.Value.position.z);

        if(Vector3.Distance(origin_point.Value, current_pos) > max_distance_origin)
        {
            target_point = origin_point.Value;
        }
        else
        {
            float pos_x = Random.Range(min_x, max_x);

            float pos_z = Random.Range(min_z, max_z);

            target_point = new Vector3(self_transform.Value.position.x + pos_x, 0, self_transform.Value.position.z + pos_z);
        }
    }
    
}
