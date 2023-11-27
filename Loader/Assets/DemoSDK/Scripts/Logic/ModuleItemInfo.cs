using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUpdateDemoSDK
{


    public class ModuleItemInfo : BundleItemInfo
    {
        

        [System.Serializable]
        public class ModuleStrings
        {
            public string string_name = "";
            public string string_value;
        }

        public List<ModuleStrings> module_strings = new List<ModuleStrings>();

        public string GetString(string string_name)
        {
            for (int i = 0; i < module_strings.Count; i++)
                if (module_strings[i].string_name.Equals(string_name) == true)
                    return module_strings[i].string_value;

            return "";
        }




        [System.Serializable]
        public class ModuleConfigs
        {
            public string config_name = "";
            public TextAsset config_file;
        }

        public List<ModuleConfigs> module_configs = new List<ModuleConfigs>();

        public TextAsset GetConfigs(string config_name)
        {
            for (int i = 0; i < module_configs.Count; i++)
                if (module_configs[i].config_name.Equals(config_name) == true)
                    return module_configs[i].config_file;

            return null;
        }

    }
}
