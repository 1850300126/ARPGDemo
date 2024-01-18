using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Config/WeaponConfigs", fileName = "WeaponConfigs")]
public class WeaponAnimationConfigs : ConfigBase
{
    public string weapon_name;
    public List<SkillConfig> light_attack_configs = new List<SkillConfig>();
    public List<SkillConfig> hard_attack_configs = new List<SkillConfig>();
}
