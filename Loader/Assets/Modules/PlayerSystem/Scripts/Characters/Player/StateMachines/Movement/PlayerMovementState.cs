using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class PlayerMovementState : IState
{        
    public PlayerMovementStateMachine movement_state_machine;
    protected readonly AirBorneStateData airborne_data;
    protected readonly GroundedStateData grounded_data;
    public PlayerMovementState(PlayerMovementStateMachine player_movement_state_machine)
    {
        movement_state_machine = player_movement_state_machine;

        airborne_data = movement_state_machine.player.player_data.air_borne_data;

        grounded_data = movement_state_machine.player.player_data.grounded_data;

        SetBaseRotationData();
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

    public virtual void OnAttackAnimationColliderOpen()
    {
        
    }

    public virtual void OnAttackAnimationColliderClose()
    {
        
    }

    public virtual void OnAttackAnimationParticlePlay()
    {
        
    }
    public virtual void OnEnter()
    {
        Debug.Log("��ǰ��״̬��" + this);
        AddInputAction();
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnHandleInput()
    {
        ReadMovementInput();
    }

    public virtual void OnFixUpdate()
    {
        Move();
    }

    public virtual void OnExit()
    {
        RemoveInputAction();
    }

    public virtual void OnTriggerEnter(Collider collider)
    {   
        
        if (movement_state_machine.player.layer_data.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);

            return;
        }
    }
    public virtual void OnTriggerStay(Collider collider)
    {
        
    }
    public virtual void OnTriggerExit(Collider collider)
    {            
        if (movement_state_machine.player.layer_data.IsGroundLayer(collider.gameObject.layer))
        {   
            OnContactWithGroundExited(collider);

            return;
        }
    }

    protected virtual void OnContactWithGround(Collider collider)
    {
        
    }

    protected virtual void OnContactWithGroundExited(Collider collider)
    {
       
    }
    protected void StartAnimation(int animationHash)
    {
        movement_state_machine.player.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        movement_state_machine.player.animator.SetBool(animationHash, false);
    }
    #endregion


    #region InputAction
    protected virtual void AddInputAction()
    {
        
    }
    protected virtual void RemoveInputAction()
    {

    }
    private void ReadMovementInput()
    {
        movement_state_machine.reusable_data.movement_input = movement_state_machine.player.player_input.player_actions.Movement.ReadValue<Vector2>();
    }
    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
        
    }        
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        
    }
    #endregion

    #region main methods
    protected void Move()
    {
        if(movement_state_machine.reusable_data.movement_input == Vector2.zero || movement_state_machine.reusable_data.MovementSpeedModifier == 0f)
        {
            return;
        }
        // 获取输入的向�?
        Vector3 move_direction = GetMovementDirection();
        // 获取�?向方�?
        float target_rot_angle = Rotate(move_direction);

        Vector3 target_rot_direction = GetTargetRotationDirection(target_rot_angle);

        float move_speed = GetMovementSpeed();

        Vector3 current_horizontal_velocity = GetCureentHorizontalVelocity();

        movement_state_machine.player.player_rb.AddForce(target_rot_direction * move_speed - current_horizontal_velocity, ForceMode.VelocityChange);
    }

    protected Vector3 GetCureentHorizontalVelocity()
    {
        Vector3 _current_velocity = movement_state_machine.player.player_rb.velocity;

        _current_velocity.y = 0;

        return _current_velocity;
    }

    protected Vector3 GetMovementDirection()
    {   
        // 在资产中�?�?设置的就�?归一化的向量
        return new Vector3(movement_state_machine.reusable_data.movement_input.x, 0, movement_state_machine.reusable_data.movement_input.y);
    }
    protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
    {
        float movementSpeed = grounded_data.BaseSpeed * movement_state_machine.reusable_data.MovementSpeedModifier;

        if (shouldConsiderSlopes)
        {
            movementSpeed *= movement_state_machine.reusable_data.MovementOnSlopesSpeedModifier;
        }

        return movementSpeed;
    }

    protected void ResetVelocity()
    {
        movement_state_machine.player.player_rb.velocity = Vector3.zero;
    }

    protected float Rotate(Vector3 direction)
    {   

        // 得到将�?�转向的�?标�?�度（融合了输入与相机的角度�?
        float direction_angle = UpdateTargetRotation(direction);

        RotateTowardsTargetRotation();

        return direction_angle;
    }
    protected Vector3 GetTargetRotationDirection(float target_rot_angle)
    {
        return Quaternion.Euler(0f, target_rot_angle, 0f) * Vector3.forward;
    }
    protected float UpdateTargetRotation(Vector3 direction, bool should_consider_cam_rot = true)
    {   
        // 得到输入产生的夹�?
        float direction_angle = GetDirectionAngle(direction);
        // 融合相机角度，使得人物�?�度能和相机正方向一�?
        if(should_consider_cam_rot)
        {
            direction_angle = AddCameraRotateAngle(direction_angle);
        }
        // 当前产生的夹角，不等于即将�?�旋�?的�?�度，就更新�?
        if(direction_angle != movement_state_machine.reusable_data.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(direction_angle);
        }

        return direction_angle;
    }
    protected void UpdateTargetRotationData(float target_angle)
    {
        movement_state_machine.reusable_data.CurrentTargetRotation.y = target_angle;

        movement_state_machine.reusable_data.DampedTargetRotationPassedTime.y = 0f;
    }
    protected void RotateTowardsTargetRotation()
    {
        float current_y_angele = GetCureentRotate().y;

        if(current_y_angele == movement_state_machine.reusable_data.CurrentTargetRotation.y)
        {
            return;
        }
        // 当当前的角度不与�?标�?�度相同时，平滑旋转
        float smoothed_y_angle = Mathf.SmoothDampAngle(current_y_angele, movement_state_machine.reusable_data.CurrentTargetRotation.y, ref movement_state_machine.reusable_data.DampedTargetRotationCurrentVelocity.y, movement_state_machine.reusable_data.TimeToReachTargetRotation.y -  movement_state_machine.reusable_data.DampedTargetRotationPassedTime.y);

        movement_state_machine.reusable_data.DampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion target_rot = Quaternion.Euler(0f, smoothed_y_angle, 0f);

        movement_state_machine.player.player_rb.MoveRotation(target_rot);

    }
    protected float GetDirectionAngle(Vector3 direction)
    {        
        // 计算出两�?值的夹�??
        float direction_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // unity的旋�?坐标�?0~360，tan值小�?0时需�?+360�?�?
        if(direction_angle < 0)
        {
            direction_angle += 360f;
        }

        return direction_angle;

    }
    protected float AddCameraRotateAngle(float angle)
    {  
        angle += movement_state_machine.player.cam_trans.eulerAngles.y;

        if(angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }
    protected Vector3 GetCureentRotate()
    {
        return movement_state_machine.player.player_rb.rotation.eulerAngles;
    }
    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, movement_state_machine.player.player_rb.velocity.y, 0f);
    }

    protected void DecelerateVertically()
    {
        Vector3 _vertical_velocity = GetPlayerVerticalVelocity();

        movement_state_machine.player.player_rb.AddForce(-_vertical_velocity * 3f, ForceMode.Acceleration);
    }

    protected void ResetVerticalVelocity()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        movement_state_machine.player.player_rb.velocity = playerHorizontalVelocity;
    }

    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = movement_state_machine.player.player_rb.velocity;

        playerHorizontalVelocity.y = 0f;

        return playerHorizontalVelocity;
    }

    protected bool IsMovingUp(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }

        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = GetPlayerHorizontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

    protected void SetBaseRotationData()
    {
        movement_state_machine.reusable_data.RotationData = grounded_data.BaseRotationData;

        movement_state_machine.reusable_data.TimeToReachTargetRotation = movement_state_machine.reusable_data.RotationData.TargetRotationReachTime;
    }        
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(movement_state_machine.reusable_data.movement_input.x, 0f, movement_state_machine.reusable_data.movement_input.y);
    }

    protected void SpiritReduce(float value)
    {
        movement_state_machine.player.player_data.self_data.spirit -= value;
    }

    protected void SpiritAdd(float value)
    {   
        if(movement_state_machine.player.player_data.self_data.spirit > movement_state_machine.player.player_data.self_data.max_energy)
        {
            return;
        }
        movement_state_machine.player.player_data.self_data.spirit += value;
    }        
    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        movement_state_machine.player.player_rb.AddForce(-playerHorizontalVelocity * movement_state_machine.reusable_data.MovementDecelerationForce, ForceMode.Acceleration);
    }

    protected void ResetAttackIndex(bool allow = true)
    {
        if(allow)
        {
            // movement_state_machine.reusable_data.next_light_combo_index = 0;
        }
    }
    #endregion


    #region attack methods
    
    protected virtual void MoveToEnemyAttack(IState next_state)
    {   

    }

    #endregion
}
