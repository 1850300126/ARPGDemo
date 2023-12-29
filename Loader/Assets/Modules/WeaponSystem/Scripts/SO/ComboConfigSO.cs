using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "WeaponAnimationConfig", menuName = "WeaponSystem/CreateWeaponAnimationConfig")]
public class ComboConfig : ScriptableObject
{   
    public string weapon_name;
    public AnimatorController controller;
    public List<LightAttackConfig> light_attack_configs = new List<LightAttackConfig>();
    public List<HardAttackConfig> hard_attack_configs = new List<HardAttackConfig>();
}
