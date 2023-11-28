using EasyUpdateDemoSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSystemModule : ModuleItem
{
    public override void OnLoaded()
    {
        WorldSystem world_system = gameObject.AddComponent<WorldSystem>();

        world_system.OnLoaded();
    } 
}
