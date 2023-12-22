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
    }

    private TaskStatus Attack()
    {   
        return TaskStatus.Success;
    }
}
