using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class MoveToTarget : EnemyConditionBase
{   

    public float stop_distance;
    public float move_speed;
    public string animator_clip_name;
    public override TaskStatus OnUpdate()
    {
        return MoveToTargetTrans();
    }
    public override void OnStart()
    {
        base.OnStart();

        InitAgent(move_speed, stop_distance);

        enemy.animator.CrossFade(animator_clip_name, 0.1f);
    
        enemy.agent.isStopped = false;
    }   
    public TaskStatus MoveToTargetTrans()
    {   
        // if(target_objcet.Value.gameObject == null) return TaskStatus.Failure;

        AgentMoveToTarget(target_objcet.Value.transform.position);

        self_transform.Value.LookAt(target_objcet.Value.transform.position);

        if(enemy.agent.remainingDistance <= stop_distance) 
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }    
}
