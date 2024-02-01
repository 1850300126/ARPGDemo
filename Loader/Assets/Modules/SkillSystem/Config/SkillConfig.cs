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
    [LabelText("���ܺ�ҡ")] public float AfterShaking = 0.8f;

    [NonSerialized, OdinSerialize]
    public SkillAnimationFrameData SkillAnimationData = new SkillAnimationFrameData();    
    [NonSerialized, OdinSerialize]
    public SkillAudioData SkillAudioData = new SkillAudioData();
    [NonSerialized, OdinSerialize]
    public SkillEffectData SkillEffectData = new SkillEffectData();
    [NonSerialized, OdinSerialize]
    public SkillCustomData SkillCustomData = new SkillCustomData();


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
