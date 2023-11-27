using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyUpdateDemoSDK
{


    [CustomEditor(typeof(ConfigSystem))]
    public class ConfigSystemInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (ConfigSystem.instance == null )
                return;

            if ( Application.isPlaying == false )
                return;

            foreach( var config_item in ConfigSystem.instance.config_files )
            {
                EditorGUILayout.LabelField("config_name :" , config_item.Key);

                for( int i = 0; i < config_item.Value.Count; i ++ )
                    EditorGUILayout.LabelField(" ---- " , config_item.Value[i]);


                EditorGUILayout.Space(5);
            }
        }


    }
}

