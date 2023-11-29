using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using Unity.VisualScripting;
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

        api_functions.Add("play_particle_in_transform", PlayParticleInTransform);

        api_functions.Add("play_particle_from_config", PlayParticleFromConfig);

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

    public object PlayParticleInTransform(object[] param)
    {   
        string name = (string)param[0];
        Vector3 _pos = (Vector3)param[1];
        Vector3 _rot = (Vector3)param[2];
        Transform _trans = (Transform)param[3];
        float delay_time = (float)param[4];

        ParticleSystem _target_particle = GetParticleFromAsset(name);

        PlayParticleInTransform(_target_particle, _pos, _rot, _trans, delay_time);

        return null;
    }
    public object PlayParticleFromConfig(object[] param)
    {   
        ParticleConfig config = (ParticleConfig)param[0];

        Transform trans = (Transform)param[1];

        if(config.particle == null)
        {
            Debug.LogWarning("粒子特效未做配置");

            return null;
        }

        ParticleSystem _particle = Instantiate(config.particle);

        _particle.AddComponent<ParticleLogic>();

        PlayParticleInTransform(_particle, config.pos, config.rot, trans, config.deley_time);

        return null;
    }
    public void PlayParticleInTransform(ParticleSystem particle, Vector3 pos, Vector3 rot, Transform trans, float delay_time = 0)
    {   
        
        ParticleSystem.MainModule main_module = particle.main;
        main_module.startDelay = delay_time;

        particle.transform.parent = trans;
        particle.transform.localPosition = pos;
        particle.transform.localRotation = Quaternion.Euler(rot);

        particle.Play();
    }

    private ParticleSystem GetParticleFromAsset(string name)
    {
        BundleInfoSystem.BundleInfoItem data = BundleInfoSystem.instance.GetBundleInfoItem(name, "VFX");

        GameObject _obj = BundleInfoSystem.LoadAddressablesPrefabs(data.data, data.name, transform);

        ParticleSystem _particle = _obj.GetComponent<ParticleSystem>(); 

        _obj.gameObject.AddComponent<ParticleLogic>();

        return _particle;
    }
}
