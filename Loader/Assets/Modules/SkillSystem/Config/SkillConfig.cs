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
    [LabelText("技能后摇")] public float AfterShaking = 0.8f;

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
