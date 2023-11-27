using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyUpdateDemoSDK
{


    [CustomEditor(typeof(APISystem))]
    public class APISystemInspector : Editor
    {


        public override void OnInspectorGUI()
        {
            if (APISystem.instance == null)
                return;

            if (Application.isPlaying == false)
                return;

            foreach (var api_item in APISystem.instance.api_functions)
            {
                EditorGUILayout.LabelField("api_functions :", api_item.Key);
                EditorGUILayout.Space(2);
            }
        }



    }
}
