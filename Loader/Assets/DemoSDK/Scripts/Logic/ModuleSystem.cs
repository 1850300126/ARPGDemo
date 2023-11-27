using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
namespace EasyUpdateDemoSDK
{

    public class ModuleSystem : MonoBehaviour
    {
        public static ModuleSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        public List<ModuleItem> module_items = new List<ModuleItem>();


        public void LoadModule(string module_name )
        {
            LoadModule(module_name, ()=> { });
        }


        public void LoadModule(string item_name, UnityAction on_load_finish )
        {
            BundleInfoSystem.BundleInfoItem data = BundleInfoSystem.instance.GetBundleInfoItem(item_name, "Module");

            if (data == null)
                return;

            LoadModule(data, on_load_finish);
        }


        public void LoadModule(BundleInfoSystem.BundleInfoItem item_data)
        {
            LoadModule(item_data, () => { });
        }


        public void LoadModule(BundleInfoSystem.BundleInfoItem item_data, UnityAction on_load_finish)
        {
            BundleInfoSystem.LoadBundleItem(item_data, module_items,transform);

            on_load_finish?.Invoke();
        }


        public void LoadModulesByTag(string module_tag , UnityAction on_load_finish)
        {
            List<BundleInfoSystem.BundleInfoItem> items = BundleInfoSystem.instance.GetBundleInfoItemsByTag(module_tag);

            for (int i = 0; i < items.Count; i++)
                    LoadModule(items[i]);

            on_load_finish?.Invoke();
        }

        public void LoadModulesByTags( List<string> module_tags, UnityAction on_load_finish)
        {

        }


    }
}
