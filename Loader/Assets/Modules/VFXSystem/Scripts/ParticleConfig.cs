using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParticleConfig
{
    /// <summary>
    /// 该动作需要使用到的特效
    /// </summary>
    public ParticleSystem particle;
    public Vector3 pos;
    public Vector3 rot;
    public float deley_time;
}
