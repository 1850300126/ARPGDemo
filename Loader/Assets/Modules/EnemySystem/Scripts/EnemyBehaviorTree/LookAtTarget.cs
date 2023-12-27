using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class LookAtTarget : EnemyConditionBase
{
    private float angleSpeed = 0.02f;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        return Look();
    }
    private TaskStatus Look()
    {   
        if(target_object.Value == null) return TaskStatus.Failure; 

        Vector3 _self = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        Vector3 _target = new Vector3(target_object.Value.transform.position.x, 0, target_object.Value.transform.position.z);
        Vector3 vec = _target - _self;
        Quaternion rotate = Quaternion.LookRotation(vec);

        enemy.transform.localRotation = Quaternion.Slerp(enemy.transform.localRotation, rotate, angleSpeed);

        // if (Vector3.Angle(vec, self_transform.Value.forward) < 0.1f)
        // {   
        //     return TaskStatus.Success;
        // }

        return TaskStatus.Running;
        
    }
}
