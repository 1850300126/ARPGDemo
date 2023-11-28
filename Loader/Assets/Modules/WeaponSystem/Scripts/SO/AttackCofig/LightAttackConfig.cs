using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LightAttackConfig
{   
    /// <summary>
    /// 衔接下一个动作的必要时间
    /// </summary>
    public float relaese_time = 0.5f;
    /// <summary>
    /// 动作切片名称
    /// </summary>
    public string light_attack_clip_name = "LightAttack";
    /// <summary>
    /// 该动作需要使用到的特效
    /// </summary>
    public List<AttackParticleConfig> particles = new List<AttackParticleConfig>();
}
