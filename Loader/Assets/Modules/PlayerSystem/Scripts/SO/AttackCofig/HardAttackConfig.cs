using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HardAttackConfig
{
    /// <summary>
    /// 衔接下一个动作的必要时间
    /// </summary>
    public float relaese_time = 0.5f;
    /// <summary>
    /// 重攻击动作列表
    /// </summary>
    public string hard_attack_clip_name = "HardAttack";
    /// <summary>
    /// 该动作需要使用到的特效
    /// </summary>
    public List<AttackParticleConfig> particles = new List<AttackParticleConfig>();
    /// <summary>
    /// 重攻击派生攻击动作列表
    /// </summary>
    public List<DeriveAttackConfig> derive_attack_clips = new List<DeriveAttackConfig>();

}
