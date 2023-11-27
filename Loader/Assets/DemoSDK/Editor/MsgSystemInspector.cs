using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyUpdateDemoSDK
{


    [CustomEditor(typeof(MsgSystem))]
    public class MsgSystemInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            if (MsgSystem.instance == null)
                return;

            if (Application.isPlaying == false)
                return;

            foreach (var msg_item in MsgSystem.instance.msg_recievers )
            {
                EditorGUILayout.LabelField("msg_name :", msg_item.Key);

                for (int i = 0; i < msg_item.Value.Count; i++)
                    EditorGUILayout.LabelField(" ---- ", msg_item.Value[i].reciever_name);


                EditorGUILayout.Space(5);
            }
        }


    }
}

