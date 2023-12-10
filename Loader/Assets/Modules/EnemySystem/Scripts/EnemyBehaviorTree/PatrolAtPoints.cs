using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PatrolAtPoints : Conditional
{
    public SharedTransform self_transform;
    public Vector3 target_point;
    public SharedVector3 origin_point;
    // 获取与远点不能超过的最大距离
    public float max_distance_origin = 10;    
    private float wait_time = 1f;
    private float wait_counter = 0f;
    private bool waiting = true;
    public float speed = 5;

    public override TaskStatus OnUpdate()
    {
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
                waiting = false;
            }
        }
        else
        {
            if(Vector3.Distance(self_transform.Value.position, target_point) < 0.01f)
            {
                wait_counter = 0f;
                waiting = true;
            }
            else
            {
                self_transform.Value.position = Vector3.MoveTowards(self_transform.Value.position, target_point, speed * Time.deltaTime);
                self_transform.Value.LookAt(target_point);
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

            target_point = new Vector3(pos_x, 0, pos_z);
        }
    }
}
