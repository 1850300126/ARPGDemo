using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DG.Tweening;
using System;

public class PatrolInPoints : EnemyConditionBase
{   
    public Vector3[] patrol_points;
    // public Vector3 target_pos;
    private int current_way_point_index = 0;
    public float wait_counter;
    public float wait_time;

    public string animator_clip_name;
    
    public override TaskStatus OnUpdate()
    {
        return Patrol();
    }

    public override void OnStart() 
    {   
        base.OnStart();

        InitAgent(2, 0.1f);

        enemy.animator.CrossFade(animator_clip_name, 0.1f);      
        

        enemy.agent.isStopped = false;  

    }

    private TaskStatus Patrol()
    {   
        Vector3 target_pos = patrol_points[current_way_point_index];

        AgentMoveToTarget(target_pos);

        self_transform.Value.LookAt(target_pos, self_transform.Value.transform.up);


        if(!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.5f)
        {   
             enemy.animator.CrossFade("Idle", 0.1f);
            wait_counter = 0f;
            current_way_point_index = (current_way_point_index + 1) % patrol_points.Length;
            return TaskStatus.Success;
        } 

        return TaskStatus.Running;
    }

    // private TaskStatus Patrol()
    // {
    //     if(waiting)
    //     {
    //         wait_counter += Time.deltaTime;
    //         if(wait_counter >= wait_time)
    //         {
    //             PlayAnimation("Walk");
    //             waiting = false;
    //         }
    //     }
    //     else
    //     {   
    //         Vector3 wp = patrol_points[current_way_point_index];

    //         AgentMoveToTarget(wp);

    //         self_transform.Value.LookAt(wp);

    //         if(!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.1f)
    //         {   
    //             PlayAnimation("Idle");
    //             wait_counter = 0f;
    //             waiting = true;
    //             current_way_point_index = (current_way_point_index + 1) % patrol_points.Length;
    //         }

    //     }
    //     return TaskStatus.Running;
    // }
}
