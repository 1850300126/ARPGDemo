using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class ObserveTarget : EnemyConditionBase
{
    public float move_speed;
    public float stop_distance;
    public string animator_clip_name;

    public override TaskStatus OnUpdate()
    {
        return Observe();
    }
    public override void OnStart()
    {
        base.OnStart();

        InitAgent(move_speed, stop_distance);

        enemy.agent.isStopped = false;

        enemy.animator.CrossFade(animator_clip_name, 0.1f);

        GetRandomPoint();
    }   

    private TaskStatus Observe()
    {   
        self_transform.Value.LookAt(target_objcet.Value.transform.position);

        if(enemy.agent.remainingDistance < 0.1f)
        {
            return TaskStatus.Success;
        }
    
        return TaskStatus.Running;
    }

    public void GetRandomPoint()
    {
        float _random = Random.Range(0f, 2f);
        
        Vector3 _random_point = enemy.transform.right * _random;

        AgentMoveToTarget(_random_point);
    }
}
