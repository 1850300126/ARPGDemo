using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAnimationConfig", menuName = "ComboAnimation/CreateComboConfig")]
public class LightAttackSO : ScriptableObject
{   
    public float relaese_time;
    public string light_attack_clip_name;
    public ParticleSystem particle;
    public List<HardAttackSO> hard_attack_list = new List<HardAttackSO>();
}
