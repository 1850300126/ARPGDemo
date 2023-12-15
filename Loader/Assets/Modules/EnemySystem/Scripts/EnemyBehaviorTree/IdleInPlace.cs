using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class IdleInPlace : EnemyConditionBase
{
    public string animator_clip_name;
    public float wait_counter;
    public float wait_time;

    public override TaskStatus OnUpdate()
    {
        return Idle();
    }

    public override void OnStart() 
    {   
        base.OnStart();

        if(target_objcet.Value != null)
            self_transform.Value.LookAt(target_objcet.Value.transform.position);

        enemy.animator.CrossFade(animator_clip_name, 0.1f);

        enemy.agent.isStopped = true;

        wait_counter = 0;
    }

    private TaskStatus Idle()
    {   
        wait_counter += Time.deltaTime;
        if(wait_counter >= wait_time)
        {   
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
