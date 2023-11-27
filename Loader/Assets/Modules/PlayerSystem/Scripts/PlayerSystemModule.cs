using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class PlayerSystemModule : ModuleItem
{
    public override void OnLoaded()
    {
        PlayerSystem player_system = gameObject.AddComponent<PlayerSystem>();

        player_system.OnLoaded();
    } 
}
