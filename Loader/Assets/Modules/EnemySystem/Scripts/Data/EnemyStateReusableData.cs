using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStateReusableData
{
    public Vector3 origin_pos;    
    public Vector3 agent_target_pos;    
    private Vector3 currentTargetRotation;
    private Vector3 timeToReachTargetRotation = new Vector3(0, 0.14f, 0);
    private Vector3 dampedTargetRotationCurrentVelocity = Vector3.zero;
    private Vector3 dampedTargetRotationPassedTime = Vector3.zero;

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
}
