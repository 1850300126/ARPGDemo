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
        api_functions.Add("create_VFX", CreateVFX);
        
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

    public object CreateVFX(object[] param)
    {
        Debug.Log(111);

        List<BundleInfoSystem.BundleInfoItem> VFX_data = BundleInfoSystem.instance.GetBundleInfoItemsByType("VFX");

        for(int i = 0;i < VFX_data.Count;i++)
        {
            VFXInfo _info = new VFXInfo
            {
                name = VFX_data[i].name,

                particle = BundleInfoSystem.LoadAddressablesAsset<ParticleSystem>(VFX_data[i].data)
            };

            _info.particle.transform.parent = this.transform;

            VFX_infos.Add(_info);
        }

        return null;
    }

}
