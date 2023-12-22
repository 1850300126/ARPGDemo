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
    private float angleSpeed = 0.05f;

    public override TaskStatus OnUpdate()
    {
        return MoveToTargetTrans();
    }
    public override void OnStart()
    {
        base.OnStart();

        InitAgent(move_speed, stop_distance);
    
        enemy.agent.isStopped = false;

        enemy.animator.CrossFade(animator_clip_name, 0.1f);
    }   
    public TaskStatus MoveToTargetTrans()
    {   
        LookAtTarget();

        AgentMoveToTarget(target_object.Value.transform.position);

        if(!enemy.agent.pathPending && enemy.agent.remainingDistance <= stop_distance) 
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }


}
