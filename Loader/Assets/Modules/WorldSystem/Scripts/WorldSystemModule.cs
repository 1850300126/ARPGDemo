using EasyUpdateDemoSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSystemModule : ModuleItem
{
    public override void OnLoaded()
    {
        WorldSystem level_system = gameObject.AddComponent<WorldSystem>();

        level_system.OnLoaded();
    } 
}
