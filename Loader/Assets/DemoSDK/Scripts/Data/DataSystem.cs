using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class DataSystem : MonoBehaviour
    {

        public static DataSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        public Dictionary<Component, Dictionary<string, object>> datas = new Dictionary<Component, Dictionary<string, object>>();


        public  void SetData(Component data_key, string data_name, object data)
        {
            if (datas.ContainsKey(data_key) == false)
                datas.Add(data_key, new Dictionary<string, object>());
            
            datas[data_key][data_name] = data;
        }

        public  Dictionary<string, object> GetData(Component data_key)
        {
            return datas.ContainsKey(data_key) == false ? new Dictionary<string, object>() : datas[data_key];
        }

        public  object GetData(Component data_key, string data_name)
        {
            return GetData<object>(data_key, data_name, null);
        }

        public  void ClearData(Component data_key)
        {
            if(datas.ContainsKey(data_key) == true)
                datas[data_key].Clear();

            RemoveData(data_key);
        }

        public  void RemoveData(Component data_key )
        {
            datas.Remove(data_key);
        }

        public  bool IsDataExist(Component data_key )
        {
            return datas.ContainsKey(data_key);
        }

        public bool IsDataExist(Component data_key, string data_name)
        {
            return datas.ContainsKey(data_key) && datas[data_key].ContainsKey(data_name);
        }


        public  bool GetDataBool(Component data_key, string data_name , bool default_data = false )
        {
            return GetData(data_key, data_name, default_data);
        }

        public  int GetDataInt(Component data_key, string data_name, int default_data = 0)
        {
            return GetData(data_key, data_name, default_data);
        }

        public  float GetDataFloat(Component data_key, string data_name, float default_data = 0.0f)
        {
            return GetData(data_key, data_name, default_data);
        }

        public  string GetDataString(Component data_key, string data_name , string default_data = "" )
        {
            return GetData(data_key, data_name, default_data);
        }


        public  T GetData<T>(Component data_key, string data_name, T defaut_value)
        {
            if (datas.ContainsKey(data_key) == false)
                return defaut_value;

            if (datas[data_key].ContainsKey(data_name) == false)
                return defaut_value;

            return (T)datas[data_key][data_name];
        }
    }
}
