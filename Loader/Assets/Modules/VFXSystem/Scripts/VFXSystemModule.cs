using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class VFXSystemModule : ModuleItem
{
    public override void OnLoaded()
    {
        VFXSystem VFX_system = gameObject.AddComponent<VFXSystem>();

        VFX_system.OnLoaded();
    } 
}

