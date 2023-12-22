using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class JudgeDistance : EnemyConditionBase
{

    public float stop_distance;

    public override TaskStatus OnUpdate()
    {
        return JudgeDistanceTarget();
    }

    public override void OnStart()
    {
        base.OnStart();
         
        AgentMoveToTarget(target_object.Value.transform.position);
    }

    public TaskStatus JudgeDistanceTarget()
    {  
        if(!enemy.agent.pathPending && enemy.agent.remainingDistance <= stop_distance) 
        {
            return TaskStatus.Failure;        
        }
        else
        {    
            return TaskStatus.Success;
        }
    }
}
