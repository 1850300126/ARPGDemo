using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackParticleConfig
{
    /// <summary>
    /// 该动作需要使用到的特效
    /// </summary>
    public string name;
    public Vector3 pos;
    public Vector3 rot;
    public float deley_time;
}
