using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using DG.Tweening;
public class CanSeeObject : EnemyConditionBase
{
    private static int _enemyLayerMask = 1 << LayerMask.NameToLayer("Character");
    public float fov_range;
    public override TaskStatus OnUpdate()
    {
        return CheckEnemyInFOVRange();
    }

    public TaskStatus CheckEnemyInFOVRange()
    {  
        // 进行判断范围内是否有可攻击的目标
        Collider[] colliders = Physics.OverlapSphere(self_transform.Value.position, fov_range , _enemyLayerMask);

        if(colliders.Length > 0)
        {   
            PlayAnimation("AttackIdle");

            reach_point.Value = colliders[0].transform.position;

            target_objcet.Value = colliders[0].gameObject;

            enemy.transform.DOLookAt(reach_point.Value, 0.14f, AxisConstraint.Y);

            enemy.animator.CrossFade("AttackIdle", 0.1f);

            return TaskStatus.Success;
        }
        else
        {   
            target_objcet.Value = null;

            return TaskStatus.Failure;
        }
        
    }
}