using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReusableData
{
    public Vector2 movement_input { get; set; }
    public Vector3 current_jump_force { get; set; }        
    public float MovementSpeedModifier { get; set; } = 1f;
    public float MovementOnSlopesSpeedModifier { get; set; } = 1f;
    public float MovementDecelerationForce { get; set; } = 1f;

    public bool ShouldSprint { get; set; }

    private Vector3 currentTargetRotation;
    private Vector3 timeToReachTargetRotation;
    private Vector3 dampedTargetRotationCurrentVelocity;
    private Vector3 dampedTargetRotationPassedTime;

    public ref Vector3 CurrentTargetRotation
    {
        get
        {
            return ref currentTargetRotation;
        }
    }

    public ref Vector3 TimeToReachTargetRotation
    {
        get
        {
            return ref timeToReachTargetRotation;
        }
    }

    public ref Vector3 DampedTargetRotationCurrentVelocity
    {
        get
        {
            return ref dampedTargetRotationCurrentVelocity;
        }
    }

    public ref Vector3 DampedTargetRotationPassedTime
    {
        get
        {
            return ref dampedTargetRotationPassedTime;
        }
    }
    public PlayerRotationData RotationData { get; set; }

    // 是否恢复体力
    public bool recharge { get; set; }
    // 消耗体力时间
    public float consume_energy ;


    # region 攻击类
    // 攻击目标
    public Transform target_trans;
    // 连招索引
    public int current_combo_index;
    # endregion
}
