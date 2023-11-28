using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

[Serializable]
public class VFXInfo
{
    public string name;
    public ParticleSystem particle;
}

public class VFXSystem : MonoBehaviour
{
    
    public Dictionary<string, APISystem.APICallFunction> api_functions = new Dictionary<string, APISystem.APICallFunction>();

    public void OnLoaded()
    {   
        // 注册api调用
        api_functions.Add("build_config", BuildConfig);
        api_functions.Add("build_vfx", BuildVFX);
        APISystem.instance.RegistAPI("VFX_system", OnSystemAPIFunction);
 
    }
    public object OnSystemAPIFunction(string function_index, object[] param)
    {
        if (api_functions.ContainsKey(function_index) == true)
        {  
            return api_functions[function_index](param);
        }
        return null;
    }

    public List<VFXInfo> VFX_infos = new List<VFXInfo>();

    // 读取所有的配置信息
    public object BuildConfig(object[] param)
    {   
 
        return null;
    }

    public object BuildVFX(object[] param)
    {   
        // 读取所有需要用到的特效
        List<BundleInfoSystem.BundleInfoItem> VFX_data = BundleInfoSystem.instance.GetBundleInfoItemsByType("VFX");
        // 实例化并且缓存到System中
        for(int i = 0;i < VFX_data.Count;i++)
        {   
            GameObject _obj = BundleInfoSystem.LoadAddressablesPrefabs(VFX_data[i].data, VFX_data[i].name, transform);

            VFXInfo _info = new VFXInfo
            {
                name = VFX_data[i].name,
                
                particle = _obj.GetComponent<ParticleSystem>()
            };

            VFX_infos.Add(_info);

            _obj.SetActive(false);
        }

        return null;
    }
}
