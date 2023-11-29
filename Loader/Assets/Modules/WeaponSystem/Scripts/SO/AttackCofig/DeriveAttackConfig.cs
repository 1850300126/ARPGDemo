using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeriveAttackConfig
{
    /// <summary>
    /// 衔接下一个动作的必要时间
    /// </summary>
    public float relaese_time = 0.5f;
    /// <summary>
    /// 派生攻击动作名称
    /// </summary>
    public string hard_attack_clip_name = "HardAttack";
    /// <summary>
    /// 该动作需要使用到的特效
    /// </summary>
    public List<ParticleConfig> particles = new List<ParticleConfig>();
}
