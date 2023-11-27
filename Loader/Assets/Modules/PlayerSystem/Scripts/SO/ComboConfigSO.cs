using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAnimationConfig", menuName = "WeaponSystem/CreateWeaponAnimationConfig")]
public class ComboConfig : ScriptableObject
{ 
    public List<LightAttackConfig> light_attack_configs = new List<LightAttackConfig>();
    public List<HardAttackConfig> hard_attack_configs = new List<HardAttackConfig>();
}
