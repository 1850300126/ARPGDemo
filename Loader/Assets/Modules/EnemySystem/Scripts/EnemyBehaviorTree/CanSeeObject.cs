using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class CanSeeObject : Conditional
{
    private static int _enemyLayerMask = 1 << LayerMask.NameToLayer("Character");
    public SharedTransform self_transform;
    public SharedTransform target_transform;
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
            target_transform.Value = colliders[0].transform;
            return TaskStatus.Success;
        }
        else
        {   
            target_transform.Value = null;
            return TaskStatus.Failure;
        }
        
    }
}