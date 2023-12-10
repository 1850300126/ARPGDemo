using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class IdleInPlace : Conditional
{
    public override TaskStatus OnUpdate()
    {
        return Idle();
    }

    public TaskStatus Idle()
    { 
        return TaskStatus.Running;
    }
}
