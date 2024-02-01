using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHitCollider : MonoBehaviour
{
    public WorldObjectBase worldObjectBase;

    [EnumFlagsAttribute]
    public AttackObjectType self_type;
    public AttackObjectType SelfType { get => self_type; set => self_type = value ; }

    private void Start() 
    {
        worldObjectBase = GetComponentInParent<WorldObjectBase>();
    }
    public void BeHit()
    {   
        worldObjectBase.BeHit();
    }
    public bool IsSelectEventType(AttackObjectType _eventType)
    {
        // 将枚举值转换为int 类型, 1 左移 
        int index = 1 << (int)_eventType;
        // 获取所有选中的枚举值
        int eventTypeResult = (int)self_type;
        // 按位 与
        if ((eventTypeResult & index) == index)
        {
            return true;
        }
        return false;
    }
}
