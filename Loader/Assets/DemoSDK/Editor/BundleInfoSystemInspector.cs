using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Burst.Intrinsics;

namespace EasyUpdateDemoSDK
{

    [CustomEditor(typeof(BundleInfoSystem))]
    public class BundleInfoSystemInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (BundleInfoSystem.instance == null)
                return;

            if (Application.isPlaying == false)
                return;

            foreach (var bundle_item in BundleInfoSystem.instance.bundle_items)
            {
                EditorGUILayout.LabelField("bundle_info :", bundle_item.Key.name);

                EditorGUILayout.LabelField(" ---- ", bundle_item.Key.type);
                EditorGUILayout.LabelField(" ---- ", bundle_item.Key.data);
                EditorGUILayout.LabelField(" ---- ", string.Join(",", bundle_item.Key.tags) );

                EditorGUILayout.Space(5);
            }
        }



    }
}

