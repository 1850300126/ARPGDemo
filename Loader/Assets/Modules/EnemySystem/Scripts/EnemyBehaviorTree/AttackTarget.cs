using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AttackTarget : EnemyConditionBase
{
    public float wait_time = 2f;
    public float wait_counter = 0f;
    public bool waiting = false;
    public float speed = 5;
    public override TaskStatus OnUpdate()
    {
        return AttackTargetObject();
    }
    public TaskStatus AttackTargetObject()
    {           
        if(waiting)
        {
            wait_counter += Time.deltaTime;
            PlayAnimation("AttackIdle");
            if(wait_counter >= wait_time)
            {
                waiting = false;
            }
        }
        else
        {   
            AgentMoveToTarget( reach_point.Value,  reach_point.Value, 0.15f, 5, 2);

            if(enemy.agent.remainingDistance >= 2.1) return TaskStatus.Running;

            PlayAnimation("Attack01");

            waiting = true;

            wait_counter = 0f;
        }
        return TaskStatus.Running;
    }   
}
