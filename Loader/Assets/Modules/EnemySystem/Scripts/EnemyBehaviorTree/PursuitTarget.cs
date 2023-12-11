using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using UnityEngine;

public class PursuitTarget : EnemyConditionBase
{
    public override TaskStatus OnUpdate()
    {
        return Pursuit();
    }

    public TaskStatus Pursuit()
    {   
        if(enemy.agent.remainingDistance <= 20f)
        {   
            PlayAnimation("Pursuit");
            reach_point.Value = target_objcet.Value.transform.position;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }    
}
