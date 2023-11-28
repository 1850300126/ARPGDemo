using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class APISystem : MonoBehaviour
    {
        public static APISystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }

        public delegate object APIFunction(string function, object[] param);
        public delegate object APICallFunction(object[] param);

        public Dictionary<string, APIFunction> api_functions = new Dictionary<string, APIFunction>();


        public void RegistAPI(string api_name, APIFunction function)
        {
            if (api_functions.ContainsKey(api_name) == false)
                api_functions.Add(api_name, function);
            else
                api_functions[api_name] = function;

            OnRecieverRegist(api_name, function);
        }

        public void RemoveAPI(string api_name)
        {
            if (api_functions.ContainsKey(api_name) == true)
                api_functions.Remove(api_name);
        }


        public object CallAPI(string api_name, string api_function, object[] param = null)
        {
            if (api_functions.ContainsKey(api_name) == true)
                return api_functions[api_name](api_function, param);

            Debug.LogWarning("-><color=#ff0000>" + "未找到该API" + "</color>");
            return null;
        }



        [System.Serializable]
        public class BulletinAPI
        {
            public string api_name = "";
            public string api_function = "";
            public object [] param;
        }

        public List<BulletinAPI> bulletin_apis = new List<BulletinAPI>();


        public void CallBulletinAPI(string api_name, string api_function, object[] param = null)
        {
            BulletinAPI msg = new BulletinAPI();
            msg.api_name = api_name;
            msg.api_function = api_function;
            msg.param = param;

            bulletin_apis.Add(msg);

            CallAPI(api_name, api_function,param);
        }


        public void ClearBulletinAPI(string api_name)
        {
            for (int i = bulletin_apis.Count - 1; i >= 0; i--)
                if (bulletin_apis[i].api_name == api_name)
                    bulletin_apis.RemoveAt(i);
        }

        public void ClearBulletinAPI(string api_name, string api_function)
        {
            for (int i = bulletin_apis.Count - 1; i >= 0; i--)
                if (bulletin_apis[i].api_name == api_name && bulletin_apis[i].api_function == api_function)
                    bulletin_apis.RemoveAt(i);
        }

        public void ClearBulletinAPI()
        {
            bulletin_apis.Clear();
        }



        public void OnRecieverRegist(string api_name, APIFunction function)
        {
            for(int i = 0; i < bulletin_apis.Count; i++)
                if (bulletin_apis[i].api_name == api_name)
                    function(bulletin_apis[i].api_function, bulletin_apis[i].param);
        }


    }
}

