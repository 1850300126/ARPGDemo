using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DG.Tweening;

public class EnemyConditionBase : Conditional
{
    public Enemy enemy;
    public SharedGameObject target_object;
    public SharedTransform self_transform;
    
    public override void OnStart()
    {
        enemy = self_transform.Value.GetComponent<Enemy>();
    }
    protected void InitAgent(float move_speed, float stop_distance)
    {
        enemy.agent.updateRotation = false;

        enemy.agent.speed = move_speed;

        enemy.agent.stoppingDistance = stop_distance;
    }
    
    protected void AgentMoveToTarget(Vector3 target)
    {   
        enemy.agent.SetDestination(target);
    }

    protected void LookAtTarget()
    {
        Vector3 _self = new Vector3(self_transform.Value.position.x, 0, self_transform.Value.position.z);
        Vector3 _target = new Vector3(target_object.Value.transform.position.x, 0, target_object.Value.transform.position.z);
        Vector3 vec = _target - _self;
        Quaternion rotate = Quaternion.LookRotation(vec);
        self_transform.Value.localRotation = Quaternion.Slerp(self_transform.Value.localRotation, rotate, 0.02f);
    }    
    protected void ResetAgentTarget()
    {   
        
    }
}
