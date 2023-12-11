using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class IdleInPlace : EnemyConditionBase
{
    public override TaskStatus OnUpdate()
    {
        return Idle();
    }

    public TaskStatus Idle()
    {   
        if(enemy.patrol)
        {
            return TaskStatus.Failure;
        }
        return TaskStatus.Running;
    }
}
