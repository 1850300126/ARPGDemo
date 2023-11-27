using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace EasyUpdateDemoSDK
{
    //notice:
    //1, do not fill data in ReadFile T class constructor function
    //  public class TestSaveData  {  public TestSaveData(){ for ( int i = 0; i < 10; i ++ ) data_list.Add( i.ToString() );}}
    // TestSaveData read_data = JsonConvert.DeserializeObject<TestSaveData>("{"data_list":["a","b","c"]}");
    // read_string : 0,1,2,3,4,5,6,7,8,9,a,b,c,


    public class SaveSystem : MonoBehaviour
    {
        public static T ReadFile<T>(string data_file , T default_data )
        {
            string fileDic = Application.persistentDataPath + "/Save/";
            string save_file = fileDic + data_file;

            if (Directory.Exists(fileDic) == false)
                Directory.CreateDirectory(fileDic);

            if (File.Exists(save_file) == false)
            {
                string save_string = JsonConvert.SerializeObject(default_data);
                StringUtility.ArchiveEncryption(ref save_string);

                File.WriteAllText(save_file, save_string, Encoding.UTF8);
            }

            string json_string = File.ReadAllText(save_file, Encoding.UTF8);
            StringUtility.ArchiveDecryption(ref json_string);

            return JsonConvert.DeserializeObject<T>(json_string);
        }


        public static void SaveFile<T>(string data_file, T save_data )
        {
            string fileDic = Application.persistentDataPath + "/Save/";
            string save_file = fileDic + data_file;

            if (Directory.Exists(fileDic) == false)
                Directory.CreateDirectory(fileDic);

            string save_string = JsonConvert.SerializeObject(save_data);

            StringUtility.ArchiveEncryption(ref save_string);
            File.WriteAllText(save_file, save_string, Encoding.UTF8);
        }






    }
}


