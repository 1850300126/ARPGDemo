using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;

[CreateAssetMenu(menuName = "Config/SkillConfig", fileName = "SkillConfig")]
public class SkillConfig : ConfigBase
{
    [LabelText("��������")] public string SkillName;
    [LabelText("֡������")] public int FrameCount = 100;
    [LabelText("֡��")] public int FrameRate = 30;

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
/// ������Ч����
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
/// ������Ч����
/// </summary>
[Serializable]
public class SkillParticleFrameData
{
    /// <summary>
    /// ����֡�¼�
    /// key:��ʼ֡��
    /// value���¼�����
    /// </summary>
    // [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    // [DictionaryDrawerSettings(KeyLabel = "��ʼ֡��", ValueLabel = "��Ч����")]
    // public Dictionary<int, SkillParticleData> FrameDataDic = new Dictionary<int, SkillParticleData>();    
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
    public List<SkillParticleData> skillParticleDatas = new List<SkillParticleData>();
}
