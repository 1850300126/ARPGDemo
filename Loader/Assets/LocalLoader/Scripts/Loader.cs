
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.AddressableAssets;
using EasyUpdateDemoSDK;
using UnityEngine.Events;
using System.Data;

namespace SimpleHotfix
{


    public class Loader : MonoBehaviour
    {
        public List<string> local_info_paths = new List<string>();

        public void Start()
        {
            OnAllDownloadSuccess();
        }

        public void OnAllDownloadSuccess()
        {
            MsgSystem.instance.SendBulletinMsg("on_download_finish", new object[] { Time.time });

            StartCoroutine(LoadItemInfo());
        }

        IEnumerator LoadItemInfo()
        {
            MsgSystem.instance.SendBulletinMsg("on_load_intem_info_start", new object[] { Time.time });
            yield return null;

            yield return ReadBundleInfoFromLocal();

            StartLoadModule();

            yield return null;

            MsgSystem.instance.SendBulletinMsg("on_load_intem_info_finish", new object[] { Time.time });
        }



        IEnumerator ReadBundleInfoFromLocal()
        {
            for (int i = 0; i < local_info_paths.Count; i++)
                BundleInfoSystem.instance.ReadBundleInfo(local_info_paths[i]);
            
            yield return null;
        }


        public void StartLoadModule()
        {
            ModuleSystem.instance.LoadModule("load_modules");
        }

















    }

}

