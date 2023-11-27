using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class PlayerSystemModule : ModuleItem
{
    public override void OnLoaded()
    {
        PlayerSystem level_system = gameObject.AddComponent<PlayerSystem>();

        level_system.OnLoaded();
    } 
}
