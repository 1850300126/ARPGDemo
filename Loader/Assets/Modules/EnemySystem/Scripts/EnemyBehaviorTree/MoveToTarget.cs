using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class MoveToTarget : EnemyConditionBase
{   

    public float stop_distance;
    public float move_speed;
    public override TaskStatus OnUpdate()
    {
        return MoveToTargetTrans();
    }

    public TaskStatus MoveToTargetTrans()
    {   
        Debug.Log(reach_point.Value);
        AgentMoveToTarget(reach_point.Value, target_objcet.Value.transform.position, 0.15f, move_speed, 0);

        self_transform.Value.LookAt(target_objcet.Value.transform.position);

        if(enemy.agent.remainingDistance <= stop_distance) return TaskStatus.Failure;

        return TaskStatus.Running;
    }    
}
