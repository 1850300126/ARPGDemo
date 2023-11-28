using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{    
    public Dictionary<string, APISystem.APICallFunction> api_functions = new Dictionary<string, APISystem.APICallFunction>();

    public void OnLoaded()
    {   
        // 注册api调用
        api_functions.Add("build_config", BuildConfig);
        api_functions.Add("build_weapon", BuildWeapon);  

        api_functions.Add("get_weapon_model", GetWeaponModel);
        api_functions.Add("get_combo_config", GetComboConfig);
        
        APISystem.instance.RegistAPI("weapon_system", OnSystemAPIFunction);
 
    }
    public object OnSystemAPIFunction(string function_index, object[] param)
    {
        if (api_functions.ContainsKey(function_index) == true)
        {  
            return api_functions[function_index](param);
        }
        return null;
    }
    // 读取所有的配置信息
    public object BuildConfig(object[] param)
    {   

 
        return null;
    }

    public object BuildWeapon(object[] param)
    {   


        return null;
    }
    public object GetWeaponModel(object[] param)
    {
        BundleInfoSystem.BundleInfoItem weapom_model = BundleInfoSystem.instance.GetBundleInfoItem((string)param[0], "weapon_model");
        if (weapom_model == null)
        {
            Debug.Log("未找到武器模型信息");
            return null;
        }

        GameObject create_weapon_model = BundleInfoSystem.LoadAddressablesPrefabs(weapom_model.data, weapom_model.name, transform);
        
        return create_weapon_model;
    }
    public object GetComboConfig(object[] param)
    {
        BundleInfoSystem.BundleInfoItem weapom_config = BundleInfoSystem.instance.GetBundleInfoItem((string)param[0], "weapon_config");
        if (weapom_config == null)
        {
            Debug.Log("未找到武器配置信息");
            return null;
        }

        ComboConfig create_weapon_model = BundleInfoSystem.LoadAddressablesAsset<ComboConfig>(weapom_config.data);
        
        return create_weapon_model;
    }
    public object GetWeaponFromData(object[] weapon_data)
    {   

        return null;
    }   
}
