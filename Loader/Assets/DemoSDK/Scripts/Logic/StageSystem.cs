using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace EasyUpdateDemoSDK
{


    public class StageSystem : MonoBehaviour
    {

        public static StageSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }

        public StageItem current_stage;
        private AsyncOperationHandle<SceneInstance> current_scene_handle;

        public void UnCurrentLoadStage(UnityAction on_load_finish)
        {
            if (current_scene_handle.IsValid())
                Addressables.UnloadSceneAsync(current_scene_handle, true).Completed += (AsyncOperationHandle<SceneInstance> sceneUnloaded_callback) => on_load_finish?.Invoke();
            else
                on_load_finish?.Invoke();
        }



        public void LoadStage(string stage_name)
        {
            LoadStage(stage_name, () => { });
        }


        public void LoadStage(string item_name, UnityAction on_load_finish)
        {
            BundleInfoSystem.BundleInfoItem data = BundleInfoSystem.instance.GetBundleInfoItem(item_name, "Scene");

            Debug.Log("LoadStage : " + item_name + "   ---   " + data );

            if (data == null)
                return;

            LoadStage(data, on_load_finish);
        }


        public void LoadStage(BundleInfoSystem.BundleInfoItem item_data, UnityAction on_load_finish)
        {
            BundleInfoSystem.instance.LoadItemAOT(item_data);

            if( item_data.data.EndsWith(".prefab") == true )
                LoadStageFromPrefab( item_data.data, item_data.name, on_load_finish);   
            else if (item_data.data.EndsWith(".unity") == true)
                StartCoroutine(LoadStageFromScene(item_data.data, on_load_finish));
        }

        public void LoadStageFromPrefab( string prefab_path , string prefab_name , UnityAction on_load_finish)
        {
            GameObject stage_obj = BundleInfoSystem.LoadAddressablesPrefabs(prefab_path, prefab_name, transform);
            StageItem stage_item = stage_obj.GetComponent<StageItem>();
            stage_item.OnLoaded();

            on_load_finish?.Invoke();
        }


        public IEnumerator LoadStageFromScene(string prefab_path, UnityAction on_load_finish)
        {
            current_scene_handle = Addressables.LoadSceneAsync(prefab_path, LoadSceneMode.Additive);

            SceneInstance scene_instance = current_scene_handle.WaitForCompletion();

            yield return scene_instance.ActivateAsync();
            SceneManager.SetActiveScene(scene_instance.Scene);
           
            yield return null;

            on_load_finish?.Invoke();
        }



    }
}
