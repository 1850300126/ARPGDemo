using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SkillAudioData
{
    /// <summary>
    /// 音效帧数据
    /// </summary>
    [NonSerialized, OdinSerialize]//不通过Unity的序列化，用Odin 的序列化
    public List<SkillAudioEvent> FrameData = new List<SkillAudioEvent>();
}
