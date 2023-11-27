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
        api_functions.Add("build_level", BuildLevel);
        
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
    public object BuildLevel(object[] param)
    {   

        StageSystem.instance.LoadStage
            (
                (string)param[0], () =>
                {   
                    LoadPlayer();
                    Debug.Log("场景加载完毕");
                }
            );
        return null;
    }

    #region 加载除地形以外的其他物体
    public void LoadElements()
    {
        // 加载elements
        // BundleInfoSystem.BundleInfoItem data = BundleInfoSystem.instance.GetBundleInfoItem(current_level_config.elements_name, "element");

        // current_level_elements_info = BundleInfoSystem.LoadAddressablesPrefabs(data.data, data.name, transform).GetComponent<CommonInfo>();
    }
    // 读取当前角色信息。 （参数后续需要根据存档来变更
    public void LoadPlayer()
    { 
        APISystem.instance.CallAPI("player_system", "build_player", new object[] { "character_GraceHoward" });
    }
    // 加载相机，相机会寻找Player标签，必须在Player之后加载
    public void LoadCameraSystem()
    {
        APISystem.instance.CallAPI("camera_system", "load_player_camera");
        APISystem.instance.CallAPI("camera_system", "load_bullet_vm");
    }
    // 玩家视角控制器，控制的是相机的点的旋转，所以要等待相机构建完成后加载。
    private void LoadPlayerControl()
    {
        APISystem.instance.CallAPI("player_control", "get_control_trans");
    }
    // 武器系统
    private void LoadWeaponSystem()
    {
        APISystem.instance.CallAPI("weapon_system", "build_weapon", new object[] { "pistol_hand_01", "pistol_01" });
    }

    private void LoadGameUI()
    {
        // ModuleSystem.instance.LoadModule("game_ui");
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
 