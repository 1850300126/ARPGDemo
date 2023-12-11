using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ObservateTarget : EnemyConditionBase
{
    public override TaskStatus OnUpdate()
    {
        return Observate();
    }

    private TaskStatus Observate()
    {
        float move_left = Random.Range(0f, 1f);

        float distance = Random.Range(5f, 8f);

        if(move_left > 0.5f)
        {
            reach_point.Value = -self_transform.Value.right * distance;
            PlayAnimation("AttackMoveLeft");
        }
        else
        {
            reach_point.Value = self_transform.Value.right * distance;
            PlayAnimation("AttackMoveRight");
        }

        return TaskStatus.Success;
    }
}
