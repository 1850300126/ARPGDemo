using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class SkillEffectData
{
    /// <summary>
    /// ����֡����
    /// key:��ʼ֡��
    /// value���¼�����
    /// </summary>
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    public List<SkillEffectEvent> FrameData = new List<SkillEffectEvent>();
}
