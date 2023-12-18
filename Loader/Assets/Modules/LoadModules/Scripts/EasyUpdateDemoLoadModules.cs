
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class EasyUpdateDemoLoadModules : ModuleItem
    {

        public override void OnLoaded()
        {
            StartCoroutine(LoadModules());
        }

        IEnumerator LoadModules()
        {
            // ModuleSystem.instance.LoadModule("ads_behavior");

            BuildAllConfig();

            yield return null;

            MsgSystem.instance.SendMsg("behavior_system_facebook_init", new object[] {});
            yield return null;

#if UNITY_ANDROID || UNITY_EDITOR
            string adjust_token =( (ModuleItemInfo)item_info).GetString("adjust_token_andriod");
#elif UNITY_IOS
            string adjust_token = ( (ModuleItemInfo)item_info).GetString("adjust_token_ios");
#endif
            MsgSystem.instance.SendMsg("behavior_system_adjust_init", new object[] { adjust_token });
            yield return null;


#if UNITY_ANDROID
            string ad_unit_in = ((ModuleItemInfo)item_info).GetString("ad_unit_in_android");
            string ad_unit_rw = ((ModuleItemInfo)item_info).GetString("ad_unit_rw_android");
            string ad_unit_open = ((ModuleItemInfo)item_info).GetString("ad_unit_open_android");
#else
            string ad_unit_in = ( (ModuleItemInfo)item_info).GetString("ad_unit_in_ios");
            string ad_unit_rw = ( (ModuleItemInfo)item_info).GetString("ad_unit_rw_ios");
            string ad_unit_open = ( (ModuleItemInfo)item_info).GetString("ad_unit_open_ios");
#endif
            string sdk_key = ((ModuleItemInfo)item_info).GetString("ad_sdk_key");

            MsgSystem.instance.SendMsg("ads_system_max_init", new object[] { sdk_key, ad_unit_in, ad_unit_rw, ad_unit_open });
            yield return null;
            
            ModuleSystem.instance.LoadModule("world_system");
           
            APISystem.instance.CallAPI("world_system", "build_config");

            yield return null;

            ModuleSystem.instance.LoadModule("pool_system");

            yield return null;
            
            ModuleSystem.instance.LoadModule("player_system");

            yield return null;

            ModuleSystem.instance.LoadModule("CM_system");
            
            yield return null;

            ModuleSystem.instance.LoadModule("VFX_system");

            yield return null;
            
            ModuleSystem.instance.LoadModule("weapon_system");

            yield return null;
            
            APISystem.instance.CallAPI("world_system", "build_world", new object[] { "EnemyTest" });
        }


        public void BuildAllConfig()
        {
            List<BundleInfoSystem.BundleInfoItem> info_items = BundleInfoSystem.instance.GetBundleInfoItemsByType("Config");

            for (int i = 0; i < info_items.Count; i++)
            {
                ConfigSystem.instance.RegistConfigItem(info_items[i].name, info_items[i].data);
            }
        }

    }
}
