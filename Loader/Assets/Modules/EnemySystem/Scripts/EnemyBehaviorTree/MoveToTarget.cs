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
        AgentMoveToTarget(target_object.Value.transform.position);

        enemy.transform.LookAt(new Vector3(target_object.Value.transform.position.x, 0, target_object.Value.transform.position.z));

        if(!enemy.agent.pathPending && enemy.agent.remainingDistance <= stop_distance) 
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }


}
