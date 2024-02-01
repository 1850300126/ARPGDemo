using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnumFlagsAttribute : PropertyAttribute {}
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        /*
         * ���ƶ�ֵö��ѡ���,0 ȫ����ѡ, -1 ȫ��ѡ��, ������ö��֮��
         * ö��ֵ = ��ǰ�±�ֵ ^ 2
         * Ĭ��[0^2 = 1 , 1 ^2 = 2,  4, 16 , .....]
         */
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);

    }
}
