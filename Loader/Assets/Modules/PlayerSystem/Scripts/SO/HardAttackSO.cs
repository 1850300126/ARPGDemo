using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAnimationConfig", menuName = "ComboAnimation/CreateHardAttackConfig")]
public class HardAttackSO : ScriptableObject
{   
    public float relaese_time;
    public string hard_attack_clip_name;
}
