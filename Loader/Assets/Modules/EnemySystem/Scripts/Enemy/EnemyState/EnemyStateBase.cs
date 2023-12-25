using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase : IState
{       
    public EnemyStateMachine enemy_state_machine;   
    public EnemyStateBase(EnemyStateMachine enemyStateMachine)
    {
        this.enemy_state_machine = enemyStateMachine;
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

    // public void OnAttackAnimationColliderOpen()
    // {
        
    // }

    // public void OnAttackAnimationColliderClose()
    // {
        
    // }

    // public void OnAttackAnimationParticlePlay()
    // {
        
    // }

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
        FindAttackTarget(collider);
    }
    public virtual void OnTriggerStay(Collider collider)
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

    protected void Rotate(Vector3 target_rot, Action rot_finish = null)
    {
        float target_angle = UpdateTargetRotation(target_rot);

        RotateTowardsTargetRotation(target_angle, rot_finish);
    }

    protected Vector3 GetEnemyCurrentPos()
    {
        return enemy_state_machine.enemy.transform.position;
    }

    protected float UpdateTargetRotation(Vector3 direction)
    {   
        
        float direction_angle = GetDirectionAngle(direction);
        Debug.Log(direction_angle);
        // 当前产生的夹角，不等于即将要旋转的角度，就更新它
        if(direction_angle != enemy_state_machine.reusable_data.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(direction_angle);
        }

        return direction_angle;
    }        
    protected void UpdateTargetRotationData(float target_angle)
    {
        enemy_state_machine.reusable_data.CurrentTargetRotation.y = target_angle;

        enemy_state_machine.reusable_data.DampedTargetRotationPassedTime.y = 0f;
    }
    protected float GetDirectionAngle(Vector3 direction)
    {        
        // 计算出两个值的夹角
        float direction_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // unity的旋转坐标是0~360，tan值小于0时需要+360矫正
        if(direction_angle < 0)
        {
            direction_angle += 360f;
        }

        return direction_angle;

    }
    protected void RotateTowardsTargetRotation(float target_angle, Action rot_finish = null)
    {
        float current_y_angele = GetCureentRotate().y;

        if(current_y_angele == target_angle)
        {   
            if(rot_finish == null) return;
            rot_finish.Invoke();
            return;
        }
        // 当当前的角度不与目标角度相同时，平滑旋转
        float smoothed_y_angle = Mathf.SmoothDampAngle(current_y_angele, target_angle, ref enemy_state_machine.reusable_data.DampedTargetRotationCurrentVelocity.y, enemy_state_machine.reusable_data.TimeToReachTargetRotation.y -  enemy_state_machine.reusable_data.DampedTargetRotationPassedTime.y);

        enemy_state_machine.reusable_data.DampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion target_rot = Quaternion.Euler(0f, smoothed_y_angle, 0f);

        enemy_state_machine.enemy.transform.rotation = target_rot;
    }

    protected bool JudgeReachTargetRot(float target_angle)
    {   
        float current_y_angele = GetCureentRotate().y;
        if(current_y_angele == target_angle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected Vector3 GetCureentRotate()
    {
        return enemy_state_machine.enemy.enemy_rb.rotation.eulerAngles;
    }



    public virtual void FindAttackTarget(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Debug.Log("find player");

            enemy_state_machine.reusable_data.agent_target_pos = collider.transform.position;

            enemy_state_machine.enemy.agent.SetDestination(enemy_state_machine.reusable_data.agent_target_pos);
            
            enemy_state_machine.ChangeState(enemy_state_machine.run_state);
        }
    }
}
