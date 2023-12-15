using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class FightIdle : Action
{   
    public Enemy enemy;
    public Animator animator;
    public SharedTransform self_transform;
    public override void OnStart() 
    {   
        base.OnStart();

        enemy = self_transform.Value.GetComponent<Enemy>();

        animator = enemy.GetComponent<Animator>();

        animator.CrossFade("AttackIdle", 0.1f);
    }
}
