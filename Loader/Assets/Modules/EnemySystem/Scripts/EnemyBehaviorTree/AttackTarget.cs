using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class AttackTarget : EnemyConditionBase
{
    public string animator_clip_name;
    public override TaskStatus OnUpdate()
    {
        return Attack();
    }

    public override void OnStart() 
    {   
        base.OnStart();

        enemy.animator.CrossFade(animator_clip_name, 0.1f);

        // enemy.agent.isStopped = true;
    }

    private TaskStatus Attack()
    {   
        // if (enemy.animator == null) {
        //     Debug.LogWarning("Animator is null");
        //     return TaskStatus.Failure;
        // }

        // var state = enemy.animator.GetCurrentAnimatorStateInfo(0);
        
        // if (state.shortNameHash == Animator.StringToHash(animator_clip_name)) {
        //     return TaskStatus.Success;
        // }

        // return TaskStatus.Running;
        return TaskStatus.Success;
    }
}
