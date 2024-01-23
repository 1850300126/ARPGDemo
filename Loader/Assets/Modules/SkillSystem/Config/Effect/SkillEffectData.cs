using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class SkillEffectData
{
    /// <summary>
    /// 动画帧数据
    /// key:开始帧数
    /// value：事件数据
    /// </summary>
    [NonSerialized, OdinSerialize]//不通过Unity的序列化，用Odin 的序列化
    public List<SkillEffectEvent> FrameData = new List<SkillEffectEvent>();
}
