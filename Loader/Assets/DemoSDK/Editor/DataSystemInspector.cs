using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyUpdateDemoSDK
{

    [CustomEditor(typeof(DataSystem))]
    public class DataSystemInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            if (DataSystem.instance == null)
                return;

            if (Application.isPlaying == false)
                return;

            foreach (var data_item in DataSystem.instance.datas )
            {
                EditorGUILayout.LabelField("data :", data_item.Key.name );

                foreach (var data_info in data_item.Value )
                    EditorGUILayout.LabelField(" ---- ", data_info.Key );


                EditorGUILayout.Space(5);
            }
        }


    }
}
