using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SkillAudioData
{
    /// <summary>
    /// ��Ч֡����
    /// </summary>
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    public List<SkillAudioEvent> FrameData = new List<SkillAudioEvent>();
}
