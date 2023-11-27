using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace EasyUpdateDemoSDK
{


    public class NetworkSystem : MonoBehaviour
    {
        public static NetworkSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        public void PostForm(string url, WWWForm form, UnityAction<string> on_success, UnityAction<string> on_failed)
        {
            StartCoroutine(PostFormCoroutine( url, form, on_success, on_failed));
        }

        public IEnumerator PostFormCoroutine(string url, WWWForm form, UnityAction<string> on_success, UnityAction<string> on_failed)
        {
            Debug.Log("post_form : " + url);

            UnityWebRequest request = UnityWebRequest.Post(url, form);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                on_failed(request.error);
            else
                    on_success(request.downloadHandler.text);
        }



        public void Get(string url, UnityAction<string> on_success, UnityAction<string> on_failed )
        {
            StartCoroutine(GetCoroutine(url, on_success, on_failed) );
        }

        public IEnumerator GetCoroutine(string url, UnityAction<string> on_success, UnityAction<string> on_failed )
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                on_failed(www.error);
            else
                on_success(www.downloadHandler.text);
        }


    }
}
