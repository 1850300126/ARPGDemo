using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase : IState
{       
    public EnemyStateMachine enemy_state_machine;
    public Vector3 origin_pos;
    public EnemyStateBase(EnemyStateMachine enemyStateMachine)
    {
        this.enemy_state_machine = enemyStateMachine;

        origin_pos = enemy_state_machine.enemy.transform.position;
    }
    
    #region IState
    public virtual void OnAnimationEnterEvent()
    {
        
    }

    public virtual void OnAnimationExitEvent()
    {
        
    }

    public virtual void OnAnimationTransitionEvent()
    {
        
    }

    public virtual void OnEnter()
    {
        Debug.Log("敌人当前的状态" + this);
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnHandleInput()
    {
        
    }

    public virtual void OnFixUpdate()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnTriggerEnter(Collider collider)
    {   
        

    }

    public virtual void OnTriggerExit(Collider collider)
    {            

    }

    protected virtual void OnContactWithGround(Collider collider)
    {
        
    }

    protected virtual void OnContactWithGroundExited(Collider collider)
    {
       
    }
    #endregion

    protected void JudgeEnemyPatrol(ref float patrol_time, bool allow = true)
    {   
        if(allow == false) return;

        patrol_time -= Time.deltaTime;

        if(patrol_time > 0) return;

        enemy_state_machine.ChangeState(enemy_state_machine.patrol_state);
    }   

    protected Vector3 GetEnemyCurrentPos()
    {
        return enemy_state_machine.enemy.transform.position;
    }


}
