using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class FightIdle : Action
{   
    public Enemy enemy;
    public Animator animator;
    public override void OnStart() 
    {   
        base.OnStart();

        enemy = this.GetComponent<Enemy>();

        enemy.animator.CrossFade("AttackIdle", 0.1f);
    }
}
