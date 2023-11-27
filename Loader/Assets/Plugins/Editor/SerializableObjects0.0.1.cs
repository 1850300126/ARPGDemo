using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class SerializableObjects : EditorWindow
{
    [SerializeField]
    public static string class_name;
    [SerializeField]
    public static string script_name;
    [SerializeField]
    public static string create_path;

    [MenuItem("EditorTool/SerializableObjects")]
    public static void CreateWindow()
    {
        GetWindow<SerializableObjects>("序列化类");
    }

    public static UnityEngine.Object objectFiled = null;

    public static object create_scripts = null;

    [SerializeField]
    public static FieldInfo[] fields_info;

    [SerializeField]
    public static List<UnityEngine.Object> components = new List<UnityEngine.Object>();

    bool is_show_scripts = false;

    bool is_show_info = false;

    private void OnGUI()
    {
        objectFiled = EditorGUILayout.ObjectField("选择一个节点：", objectFiled, typeof(GameObject), true);

        if (GUILayout.Button("获取节点上的所有脚本", GUILayout.Height(30), GUILayout.Width(150)))
        {
            components.Clear();

            GetScripts();

            is_show_scripts = true;
        }

        ShowScripts();

        ShowFieldsInfo();

        GUILayout.Label("请将需要创建的路径填入");

        create_path = GUILayout.TextField(create_path);

        if (GUILayout.Button("关闭", GUILayout.Height(30), GUILayout.Width(100)))//通过后面GUILayoutOption参数自定义控件大小
        {
            Close();//关闭窗口
        }
    }

    public void ShowScripts()
    {
        if (is_show_scripts)
        {
            GUILayout.Label("点击该节点下获取的脚本名，将获取该脚本中的字段信息");

            if (components == null) return;

            for (int i = 0; i < components.Count; i++)
            {
                if (GUILayout.Button(components[i].GetType().Name, GUILayout.Height(30), GUILayout.Width(250)))
                {
                    GetFieldInfo(components[i].GetType().Name);

                    is_show_info = true;
                }
            }
        }
    }

    public void ShowFieldsInfo()
    {
        if (is_show_info)
        {
            GUILayout.Label("点击该脚本下的获取到的字段名称，将在指定位置生成该字段的json文件");

            if (fields_info == null) return;

            for (int i = 0; i < fields_info.Length; i++)
            {
                if (GUILayout.Button(fields_info[i].Name, GUILayout.Height(30), GUILayout.Width(400)))
                {
                    Serializable(fields_info[i].Name);
                }
            }
        }

    }

    public static void GetScripts()
    {
        GameObject game_object = (GameObject)objectFiled;

        MonoBehaviour[] scripts = game_object.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour mb in scripts)
        {
            components.Add(mb);
        }
    }

    public static object target_component;
    public static void GetFieldInfo(string script_name)
    {
        target_component = null;

        foreach (object comp in components)
        {
            if(comp.GetType().Name == script_name)
            {
                target_component = comp;
                Debug.Log(comp.GetType().Name);
            }
        }
        fields_info = target_component.GetType().GetFields();
    }


    public static void Serializable(string class_name)
    {
        if (class_name == null)
        {
            Debug.Log("输入的字段名为空");
            return;
        }

        FieldInfo data = null;

        foreach (FieldInfo target_class in fields_info)
        {
            Debug.Log(target_class.Name);

            if (target_class.Name == class_name)
            {
                data = target_component.GetType().GetField(class_name);
            }
        }

        object get_data = data.GetValue(target_component);

        JsonSerializerSettings settings = new JsonSerializerSettings();

        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        string jsonStr = JsonConvert.SerializeObject(get_data, settings);

        StreamWriter fileStream;

        if (create_path == null)
        {
            fileStream = new StreamWriter(Application.dataPath + "/" + class_name + ".json", true);
        }
        else
        {
            fileStream = new StreamWriter(Application.dataPath + create_path + "/" + class_name + ".json", true);
        }

        fileStream.Write(jsonStr);

        fileStream.Close();

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
}
