using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class WeaponSystemModule : ModuleItem
{   
    public override void OnLoaded()
    {
        WeaponSystem weapon_system = gameObject.AddComponent<WeaponSystem>();

        weapon_system.OnLoaded();
    } 
}
