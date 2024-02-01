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
        // ��ö��ֵת��Ϊint ����, 1 ���� 
        int index = 1 << (int)_eventType;
        // ��ȡ����ѡ�е�ö��ֵ
        int eventTypeResult = (int)self_type;
        // ��λ ��
        if ((eventTypeResult & index) == index)
        {
            return true;
        }
        return false;
    }
}
