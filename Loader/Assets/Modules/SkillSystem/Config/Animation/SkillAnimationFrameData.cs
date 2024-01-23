using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;
/// <summary>
/// 技能动画数据
/// </summary>
[Serializable]
public class SkillAnimationFrameData
{
    /// <summary>
    /// 动画帧数据
    /// key:开始帧数
    /// value：事件数据
    /// </summary>
    [NonSerialized, OdinSerialize]//不通过Unity的序列化，用Odin 的序列化
    [DictionaryDrawerSettings(KeyLabel = "开始帧", ValueLabel = "动画数据")]
    public Dictionary<int, SkillAnimationClipData> FrameDataDic = new Dictionary<int, SkillAnimationClipData>();
}
