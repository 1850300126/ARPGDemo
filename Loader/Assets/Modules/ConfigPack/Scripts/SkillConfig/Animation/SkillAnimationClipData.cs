using UnityEngine;

public class SkillAnimationClipData : SkillFrameEventBase
{
    public AnimationClip AnimationClip;
    public float TransitionTime = 0.25f;
    public bool ApplyRootMotion;

#if UNITY_EDITOR
    public int DurationFrame;

#endif

}
