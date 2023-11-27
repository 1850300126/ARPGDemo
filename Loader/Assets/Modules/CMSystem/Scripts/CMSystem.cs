using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using EasyUpdateDemoSDK;
using UnityEngine;

public class CMSystem : MonoBehaviour
{
    public Dictionary<string, APISystem.APICallFunction> api_functions = new Dictionary<string, APISystem.APICallFunction>();

    public CinemachineVirtualCamera player_CM;

    public void OnLoaded()
    {   
        // 注册api调用
        // 构建场景类
        api_functions.Add("build_CM", BuildCM);

        api_functions.Add("CM_find_player", FindPlayer);
        
        APISystem.instance.RegistAPI("CM_system", OnLevelSystemAPIFunction);

    }

    public object OnLevelSystemAPIFunction(string function_index, object[] param)
    {
        if (api_functions.ContainsKey(function_index) == true)
        {
            return api_functions[function_index](param);
        }
        return null;
    }

    public object BuildCM(object[] param)
    {   
        ModuleItemInfo CM_system_info = GetComponent<ModuleItemInfo>();

        player_CM = CM_system_info.GetPoint("player_CM").GetComponent<CinemachineVirtualCamera>();

        return null;
    }

    public object FindPlayer(object[] param)
    {
        Player _player = (Player)APISystem.instance.CallAPI("player_system", "get_player");

        player_CM.m_Follow = _player.transform.Find("CM_point");

        player_CM.m_LookAt = _player.transform.Find("CM_point");

        return null;
    }
}
