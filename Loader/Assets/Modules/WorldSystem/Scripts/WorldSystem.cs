using EasyUpdateDemoSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class WorldConfig
{
    public string world_name = "DemoSpace";
}

public class WorldSystem : MonoBehaviour
{   
    // 关卡系统api集合
    public Dictionary<string, APISystem.APICallFunction> api_functions = new Dictionary<string, APISystem.APICallFunction>();

    public void OnLoaded()
    {   
        // 注册api调用
        // 构建场景类
        api_functions.Add("build_config", BuildConfig);
        api_functions.Add("build_world", BuildWorld);
        
        APISystem.instance.RegistAPI("world_system", OnLevelSystemAPIFunction);

    }

    public object OnLevelSystemAPIFunction(string function_index, object[] param)
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

    // 得到当前关卡需要用到的配置信息
    public WorldConfig current_world_config;
    public CommonInfo current_level_elements_info;
    public object BuildWorld(object[] param)
    {   

        StageSystem.instance.LoadStage
            (
                (string)param[0], () =>
                {   
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Debug.Log("场景加载完毕");
                    LoadVFXSystem();
                    LoadWeaponSystem();
                    LoadPlayer();
                    LoadCMSystem();
                }
            );
        return null;
    }

    #region 加载除地形以外的其他物体
    public void LoadElements()
    {

    }
   
    public void LoadVFXSystem()
    {
        APISystem.instance.CallAPI("VFX_system", "build_vfx");
        Debug.Log("特效粒子系统加载完毕");        
    }
    public void LoadWeaponSystem()
    {
        APISystem.instance.CallAPI("weapon_system", "build_config");
        Debug.Log("武器系统加载完毕");        
    }
    public void LoadPlayer()
    { 
        APISystem.instance.CallAPI("player_system", "build_player", new object[] { "character_GraceHoward" });
        Debug.Log("角色加载完毕");
    }

    public void LoadCMSystem()
    {
        APISystem.instance.CallAPI("CM_system", "build_CM");
        APISystem.instance.CallAPI("CM_system", "CM_find_player");
        Debug.Log("相机加载完毕");
    }


    #endregion

    #region 卸载地形等物体
    public object UnloadLevel(object[] param)
    {   
        StartCoroutine(UnloadStage());
        return null;
    }
    // 卸载该关卡的物品
    public IEnumerator UnloadStage()
    {
        SceneManager.UnloadSceneAsync(current_world_config.world_name);
        Destroy(current_level_elements_info.gameObject);
        current_level_elements_info = null;

        yield return null;

        APISystem.instance.CallAPI("move_point_system", "unload");

        yield return null;
    }

    #endregion

}
 