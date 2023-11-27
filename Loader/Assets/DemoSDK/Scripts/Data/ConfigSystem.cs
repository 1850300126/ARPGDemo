using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace EasyUpdateDemoSDK
{


    public class ConfigSystem : MonoBehaviour
    {
        public static ConfigSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        public Dictionary<string, List<string>> config_files = new Dictionary<string, List<string>>();



        public void RegistConfigItem( string config_name , string config_file_path )
        {
            if(  config_files.ContainsKey( config_name ) == false )
                config_files.Add( config_name , new List<string>() );

            config_files[config_name].Add(config_file_path);
        }



        public List<T> BuildConfig<T>(string config_name)
        {
            List<T> config_list = new List<T>();

            foreach (var config in config_files)
            {
                if (config.Key == config_name)
                    for (int i = 0; i < config.Value.Count; i++)
                    {
                        TextAsset info_asset = Addressables.LoadAssetAsync<TextAsset>(config.Value[i]).WaitForCompletion();

                        if (info_asset != null)
                            config_list.Add(JsonConvert.DeserializeObject<T>(info_asset.text));
                    }
            }

            return config_list;
        }



        public T BuildConfigFromFile<T>( string file_path )
        {
            TextAsset info_asset = Addressables.LoadAssetAsync<TextAsset>(file_path).WaitForCompletion();

            return JsonConvert.DeserializeObject<T>(info_asset.text);
        }




        public string GetCommonConfig( string param_name , string default_string )
        {
            List<Dictionary<string, string>> config_list = BuildConfig<Dictionary<string, string>>("common_config");

            for( int i = 0; i < config_list.Count; i ++ )
                if(config_list[i].ContainsKey(param_name))
                    return config_list[i][param_name];

            return default_string;
        }








    }
}

