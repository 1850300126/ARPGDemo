using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectEvent
{
#if UNITY_EDITOR
    public string TrackName = "��Ч���";
#endif
    public int FrameIndex = -1;
    public GameObject Prefab;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    public float Duration;
    /// <summary>
    /// �Ƿ��Զ�����
    /// </summary>
    public bool AutoDestruct;

}
