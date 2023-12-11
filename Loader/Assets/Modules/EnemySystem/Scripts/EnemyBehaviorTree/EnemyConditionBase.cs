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
    public SharedVector3 origin_point;
    
    public override void OnStart()
    {
        enemy = self_transform.Value.GetComponent<Enemy>();
    }

    protected void PlayAnimation(string name)
    {
        // Debug.Log(enemy.animator.GetCurrentAnimatorStateInfo(0).fullPathHash.ToString());

        if(enemy.animator.GetCurrentAnimatorStateInfo(0).fullPathHash.ToString().Equals(name)) return;

        enemy.animator.CrossFade(name, 0.1f);
    }
    protected void AgentMoveToTarget(Vector3 target, Vector3 look_target, float rotate_speed, float move_speed, float stop_distance)
    {   
        if(enemy.agent.pathPending) return;
        enemy.agent.SetDestination(target);
        enemy.agent.updateRotation = false;
        enemy.agent.speed = move_speed;
        enemy.agent.stoppingDistance = stop_distance;
        enemy.transform.DOLookAt(look_target, rotate_speed, AxisConstraint.Y);
    }
    protected void ResetAgentTarget()
    {   
        
    }
}
