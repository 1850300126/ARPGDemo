using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    // 关卡系统api集合
    public Dictionary<string, APISystem.APICallFunction> api_functions = new Dictionary<string, APISystem.APICallFunction>();
    public Player player;
    public void OnLoaded()
    {
        // 注册api调用
        // 构建场景类
        // api_functions.Add("build_config", BuildConfig);
        api_functions.Add("build_player", BuildPlayer);

        api_functions.Add("get_player", GetPlayer);
        
        APISystem.instance.RegistAPI("player_system", OnPlayerSystemAPIFunction);
    }

    
    public object OnPlayerSystemAPIFunction(string function_index, object[] param)
    {
        if (api_functions.ContainsKey(function_index) == true)
        {
            return api_functions[function_index](param);
        }
        return null;
    }

    private object BuildConfig(object[] param)
    {
        return null;
    }

    private object BuildPlayer(object[] param)
    {
        CreatePlayer((string)param[0], Vector3.zero, Vector3.zero);

        return null;
    }

    public void CreatePlayer(string name, Vector3 pos, Vector3 rot)
    {
        BundleInfoSystem.BundleInfoItem model_data = BundleInfoSystem.instance.GetBundleInfoItem(name, "character");
            if (model_data == null)
            {
                Debug.Log("未找到角色模型");
                return;
            }

        BundleInfoSystem.BundleInfoItem player_config = BundleInfoSystem.instance.GetBundleInfoItem("player_config", "player_config");
            if (player_config == null)
            {
                Debug.Log("未找到角色配置信息");
                return;
            }

        GameObject create_character_model = BundleInfoSystem.LoadAddressablesPrefabs(model_data.data, model_data.name, transform);

        player = create_character_model.AddComponent<Player>();

        player.player_data = BundleInfoSystem.LoadAddressablesAsset<PlayerSO>(player_config.data);

        player.OnLoaded();
    }

    public object GetPlayer(object param)
    {   
        if(player != null)
        {
            return player;
        }
        else
        {   
            Warning.Info("未找到角色引用");
            return null;
        }
    }
}
