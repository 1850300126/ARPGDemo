using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class CharacterBeHit : EnemyConditionBase
{   
    public TaskStatus status = TaskStatus.Failure;
    public string animator_clip_name;

    public void BeHit()
    {   
        enemy.animator.CrossFade("BeHit", 0.1f);
    }


    
    public override TaskStatus OnUpdate()
    {
        return status;
    }
}
