using EasyUpdateDemoSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystemModule : ModuleItem
{
    public override void OnLoaded()
    {
        PoolSystem pool_system = gameObject.AddComponent<PoolSystem>();

        pool_system.OnLoaded();
    }
}
