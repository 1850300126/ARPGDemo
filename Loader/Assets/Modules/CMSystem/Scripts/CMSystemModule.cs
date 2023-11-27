using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class CMSystemModule : ModuleItem
{
    public override void OnLoaded()
    {
        CMSystem CM_system = gameObject.AddComponent<CMSystem>();

        CM_system.OnLoaded();
    } 
}
