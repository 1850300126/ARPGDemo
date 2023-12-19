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
        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, fov_range , _enemyLayerMask);

        if(colliders.Length > 0)
        {   
            fov_range = 20f;

            target_object.Value = colliders[0].gameObject;

            return TaskStatus.Success;
        }
        else
        {   
            fov_range = 6;
            
            target_object.Value = null;

            return TaskStatus.Failure;
        }
        
    }
}