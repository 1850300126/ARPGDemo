using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

public class SkillCustomData
{
    /// <summary>
    /// ����֡����
    /// key:��ʼ֡��
    /// value���¼�����
    /// </summary>
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    public List<SkillCustomEvent> FrameData = new List<SkillCustomEvent>();
}
