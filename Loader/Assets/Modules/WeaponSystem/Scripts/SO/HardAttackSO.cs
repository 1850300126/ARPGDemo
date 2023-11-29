using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAnimationConfig", menuName = "ComboAnimation/CreateHardAttackConfig")]
public class HardAttackSO : ScriptableObject
{   
    public float relaese_time;
    public string hard_attack_clip_name;    
    /// <summary>
    /// 该动作需要使用到的特效
    /// </summary>
    public List<ParticleConfig> particles = new List<ParticleConfig>();
}
