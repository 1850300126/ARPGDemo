using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DG.Tweening;

public class EnemyConditionBase : Conditional
{
    public Enemy enemy;
    public SharedGameObject target_objcet;
    public SharedTransform self_transform;
    public SharedVector3 reach_point;
    
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
        if(enemy.agent.pathPending) return;
        enemy.agent.SetDestination(target);
    }
    protected void ResetAgentTarget()
    {   
        
    }
}
