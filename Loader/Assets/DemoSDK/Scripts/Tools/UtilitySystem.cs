using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyUpdateDemoSDK
{

    public class UtilitySystem : MonoBehaviour
    {
        public static GameObject GetAnItemFromList(GameObject template, List<GameObject> item_list, int from_index = 0)
        {
            for (int i = from_index; i < item_list.Count; i++)
                if (item_list[i].gameObject.activeInHierarchy == false)
                    return item_list[i];

            GameObject new_item = Instantiate(template, template.transform.parent);
            item_list.Add(new_item);

            return new_item;
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


        public static Type FindType(string typeName, bool useFullName = false, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            StringComparison e = (ignoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (useFullName)
            {
                foreach (var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (string.Equals(t.FullName, typeName, e)) return t;
                    }
                }
            }
            else
            {
                foreach (var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (string.Equals(t.Name, typeName, e) || string.Equals(t.FullName, typeName, e)) return t;
                    }
                }
            }
            return null;
        }


        public static T AddAndGetComponent<T>( string class_name , GameObject game_obj )
        {
            game_obj.AddComponent( FindType(class_name) );

            return game_obj.GetComponent<T>();
        }





        public static int[] GetIntArray( string[] string_array)
        {
            int[] int_array = new int[string_array.Length];

            for (int i = 0; i < string_array.Length; i++)
                int.TryParse(string_array[i], out int_array[i]);

            return int_array;
        }



        public static string GetDataFromDictionary(string data_name, Dictionary<string,string> data, string default_data = "" )
        {
            return data.ContainsKey(data_name) == true ? data[data_name] :default_data;
        }

        public static float GetDataFromDictionary(string data_name, Dictionary<string, string> data, float default_data = 0)
        {
            return data.ContainsKey(data_name) == true ? float.Parse( data[data_name]) : default_data;
        }

        public static int GetDataFromDictionary(string data_name, Dictionary<string, string> data, int default_data = 0)
        {
            return data.ContainsKey(data_name) == true ? int.Parse(data[data_name]) : default_data;
        }


        public static bool GetDataFromDictionary(string data_name, Dictionary<string, string> data, bool default_data = false )
        {
            return data.ContainsKey(data_name) == true ? bool.Parse(data[data_name]) : default_data;
        }



        public static Vector3 PositionWithSameY( Transform from , Transform same_y )
        {
            Vector3 position = from.position;  position.y = same_y.position.y;

            return position;
        }

        public static Vector3 DirectionWith0Y(Transform from, Transform to )
        {
            Vector3 position = to.position - from.position;   position.y = 0;

            return position;
        }


        public static T2 GetNearestInList<T1,T2>(T1 from_what, List<T2> to_what) where T1 : Component where T2 : Component
        {
            if (to_what.Count <= 0 )
                return null;

            float min_distance = Vector3.Distance(to_what[0].transform.position, from_what.transform.position); ;
            T2 target_grid = to_what[0];

            for (int i = 1; i < to_what.Count; i++)
            {
                float distance = Vector3.Distance(to_what[i].transform.position, from_what.transform.position);
                if (distance < min_distance)
                {
                    min_distance = distance;
                    target_grid = to_what[i];
                }
            }

            return target_grid;
        }



    }
}
