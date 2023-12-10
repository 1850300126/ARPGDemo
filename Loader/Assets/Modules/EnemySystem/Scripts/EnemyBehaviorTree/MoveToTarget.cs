using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class MoveToTarget : Conditional
{   
    public SharedTransform self_transform;
    public SharedTransform target_transform;
    public float speed = 5;
    public override TaskStatus OnUpdate()
    {
        return MoveToTargetTrans();
    }

    public TaskStatus MoveToTargetTrans()
    {   

        self_transform.Value.position = Vector3.MoveTowards(self_transform.Value.position, target_transform.Value.position, speed * Time.deltaTime);
        self_transform.Value.LookAt(target_transform.Value.position);
        // 在到达目标前，索敌范围内，会一直追击。
        if(Vector3.Distance(self_transform.Value.position, target_transform.Value.position) < 10f)
        {
            if(Vector3.Distance(self_transform.Value.position, target_transform.Value.position) > 0.1f)
            {
                return TaskStatus.Running;
            }
        }
        return TaskStatus.Failure;
    }    
}
