
#if  !NOT_USE_HYBIRDCLR
using HybridCLR;
#endif

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace EasyUpdateDemoSDK
{

    public class BundleInfoSystem : MonoBehaviour
    {
        public static BundleInfoSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        [System.Serializable]
        public class BundleInfoItem
        {
            public string name = "";
            public string type = "";
            public string data = "";
            public string[] tags = new string[] { };
        }


        [System.Serializable]
        public class BundleData
        {
            public string name = "";
            public string path = "";
            public string[] aot = new string[] { };
            public string[] dll = new string[] { };
            public BundleInfoItem[] datas = new BundleInfoItem[] { };
        }

        public Dictionary<BundleInfoItem, BundleData> bundle_items = new Dictionary<BundleInfoItem, BundleData>();


        public void ReadBundleInfo(string info_path )
        {
            TextAsset info_asset = Addressables.LoadAssetAsync<TextAsset>(info_path).WaitForCompletion();

            if (info_asset != null)
                ReadBundleData(JsonConvert.DeserializeObject<BundleData>(info_asset.text));
        }

        public void ReadBundleData( BundleData bundle_data )
        {
            for (int i = 0; i < bundle_data.datas.Length; i++)
            {
                bundle_data.datas[i].data = bundle_data.path + "/" + bundle_data.datas[i].data;

                bundle_items.Add(bundle_data.datas[i], bundle_data);
            }
        }


        public BundleInfoItem GetBundleInfoItem(string item_name, string item_type)
        {
            foreach (var item in bundle_items)
                if (item.Key.name == item_name && item.Key.type == item_type)
                    return item.Key;

            return null;
        }

        public List<BundleInfoItem> GetBundleInfoItemsByType( string item_type)
        {
            List<BundleInfoItem> infos = new List<BundleInfoItem>();

            foreach (var item in bundle_items)
                if ( item.Key.type == item_type)
                    infos.Add(item.Key);

            return infos;
        }


        public List<BundleInfoItem> GetBundleInfoItemsByTag(string item_tag)
        {
            List<BundleInfoItem> items = new List<BundleInfoItem>();

            foreach (var item in bundle_items)
                if (System.Array.IndexOf(item.Key.tags, item_tag) >= 0)
                    items.Add(item.Key);

            return items;
        }


        public List<BundleInfoItem> GetBundleInfoItemsByName(string item_name )
        {
            List<BundleInfoItem> items = new List<BundleInfoItem>();

            foreach (var item in bundle_items)
                if ( item.Key.name == item_name )
                    items.Add(item.Key);

            return items;
        }




        BundleInfoItem last_load_item;
        public void LoadItemAOT(BundleInfoItem item)
        {
            if (last_load_item == item)
                return;

            if (bundle_items.ContainsKey(item))
            {
                last_load_item = item;

                LoadItemAOT(bundle_items[item]);
            }
        }


        public static void LoadItemAOT(BundleData item_data)
        {

#if !NOT_USE_HYBIRDCLR
            if (item_data.aot.Length > 0)
            {
                HomologousImageMode mode = HomologousImageMode.SuperSet;

                for (int j = 0; j < item_data.aot.Length; j++)
                {
                    TextAsset aot_asset = Addressables.LoadAssetAsync<TextAsset>(item_data.path + "/Dlls/" + item_data.aot[j]).WaitForCompletion();

                    LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(aot_asset.bytes, mode);

                    Debug.Log("aot: " + item_data.path + "/Dlls/" + item_data.aot[j] + "   error:" + err);
                }
            }

            if (item_data.dll.Length > 0)
            {
                for (int j = 0; j < item_data.dll.Length; j++)
                {
                    TextAsset dll_asset = Addressables.LoadAssetAsync<TextAsset>(item_data.path + "/Dlls/" + item_data.dll[j]).WaitForCompletion();

                    Debug.Log("Assembly.Load: " + item_data.path + "/Dlls/" + item_data.dll[j]);

                    Assembly.Load(dll_asset.bytes);
                }
            }
#endif
        }



        public static GameObject LoadAddressablesPrefabs(string prefab_full_path, string obj_name, Transform parent)
        {
            GameObject go = Addressables.LoadAssetAsync<GameObject>(prefab_full_path).WaitForCompletion();

            GameObject game_obj = Instantiate(go, parent);
            game_obj.name = obj_name;

            return game_obj;
        }

        public static T LoadAddressablesAsset<T>(string full_path )
        {
            return Addressables.LoadAssetAsync<T>(full_path).WaitForCompletion();
        }



        public static IEnumerator LoadAddressableScene(string scene_full_path, UnityAction on_load_finish)
        {
            var scene_handle = Addressables.LoadSceneAsync(scene_full_path, LoadSceneMode.Additive);

            SceneInstance scene_instance = scene_handle.WaitForCompletion();
            yield return scene_instance.ActivateAsync();

            SceneManager.SetActiveScene(scene_instance.Scene);

            on_load_finish?.Invoke();
        }



        public static T GetAnItemFromList<T>(T template, List<T> item_list, int from_index = 0) where T : Component
        {
            for (int i = from_index; i < item_list.Count; i++)
                if (item_list[i].gameObject.activeInHierarchy == false)
                    return item_list[i];

            T new_item = Instantiate(template, template.transform.parent);
            item_list.Add(new_item);

            return new_item;
        }


        public static T LoadAnItemFromDictionary<T>(string item_name, string type_name, Dictionary<string, List<T>> item_dictionary , Transform item_root ) where T : BundleItem 
        {
            if (item_dictionary.ContainsKey(item_name) == false)
            {
                BundleInfoItem bundle_info = BundleInfoSystem.instance.GetBundleInfoItem(item_name, type_name );

                if (bundle_info == null)
                    return null;

                GameObject hero_obj = LoadAddressablesPrefabs(bundle_info.data, bundle_info.name, item_root);

                BundleItemInfo item_info = hero_obj.GetComponent<BundleItemInfo>();
                item_info.OnLoaded();

                item_dictionary.Add(item_name, new List<T>() { (T)item_info.bundle_item });

                hero_obj.SetActive(false);
            }

            return GetAnItemFromList(item_dictionary[item_name][0], item_dictionary[item_name], 1);
        }




        public static void LoadBundleItem<T>( BundleInfoItem item_data, List<T> item_list, Transform item_root ) where T : BundleItem
        {
            BundleInfoSystem.instance.LoadItemAOT(item_data);

            GameObject module_obj = BundleInfoSystem.LoadAddressablesPrefabs(item_data.data, item_data.name, item_root);

            BundleItemInfo item_info = module_obj.GetComponent<BundleItemInfo>();
            item_info.OnLoaded();

            item_list.Add( (T) item_info.bundle_item);
        }






    }
}
