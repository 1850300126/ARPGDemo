using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;

[CreateAssetMenu(menuName = "Config/SkillConfig", fileName = "SkillConfig")]
public class SkillConfig : ConfigBase
{
    [LabelText("技能名称")] public string SkillName;
    [LabelText("帧数上限")] public int FrameCount = 100;
    [LabelText("帧率")] public int FrameRate = 30;

    [NonSerialized, OdinSerialize]
    public SkillAnimationFrameData SkillAnimationData = new SkillAnimationFrameData();    
    [NonSerialized, OdinSerialize]
    public SkillAudioData SkillAudioData = new SkillAudioData();
    [NonSerialized, OdinSerialize]
    public SkillEffectData SkillEffectData = new SkillEffectData();

#if UNITY_EDITOR
    private static Action onSkillConfigValidate;

    public static void SetValidateAction(Action action)
    {
        onSkillConfigValidate = action;
    }

    private void OnValidate()
    {
        onSkillConfigValidate?.Invoke();
    }
#endif

}

/// <summary>
/// 技能特效数据
/// </summary>
public class SkillParticleData : SkillFrameEventBase
{
    public ParticleSystem particlePrefab;

#if UNITY_EDITOR
    public int StartFrame;
    public int DurationFrame;


#endif    
}

/// <summary>
/// 技能特效数据
/// </summary>
[Serializable]
public class SkillParticleFrameData
{
    /// <summary>
    /// 动画帧事件
    /// key:开始帧数
    /// value：事件数据
    /// </summary>
    // [NonSerialized, OdinSerialize]//不通过Unity的序列化，用Odin 的序列化
    // [DictionaryDrawerSettings(KeyLabel = "开始帧数", ValueLabel = "特效数据")]
    // public Dictionary<int, SkillParticleData> FrameDataDic = new Dictionary<int, SkillParticleData>();    
    [NonSerialized, OdinSerialize]//不通过Unity的序列化，用Odin 的序列化
    [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
    public List<SkillParticleData> skillParticleDatas = new List<SkillParticleData>();
}
