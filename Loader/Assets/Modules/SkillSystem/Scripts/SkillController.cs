using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private AnimationController animationController;

    private bool isPlaying = false;     //当前是否处于播放状态
    public bool IsPlaying { get => isPlaying; }


    private SkillConfig skillConfig;    //当前播放的技能配置
    private int currentFrameIndex;      //当前是第几帧
    private float playTotalTime;        //当前播放的总时间
    private int frameRate;              //当前技能的帧率

    public void Init(AnimationController animationController)
    {
        this.animationController = animationController;
    }

    private Action<Vector3, Quaternion> rootMotionAction;
    private Action skillEndAction;

    /// <summary>
    /// 播放技能
    /// </summary>
    /// <param name="skillConfig"> 技能配置 </param>
    public void PlaySkill(SkillConfig skillConfig, Action skillEndAction, Action<Vector3, Quaternion> rootMotionAction = null)
    {
        this.skillConfig = skillConfig;
        this.skillEndAction = skillEndAction;
        this.rootMotionAction = rootMotionAction;

        currentFrameIndex = -1;
        frameRate = skillConfig.FrameRate;
        playTotalTime = 0;
        isPlaying = true;

        TickSkill();
    }

    private void Update()
    {
        if (isPlaying)
        {
            playTotalTime += Time.deltaTime;
            //根据总时间判断当前是第几帧
            int targetFrameIndex = (int)(playTotalTime * frameRate);
            //防止一帧延迟过大，追帧
            while (currentFrameIndex < targetFrameIndex)
            {
                //驱动一次技能
                TickSkill();
            }

            //如果达到最后一帧，技能结束
            if (targetFrameIndex >= skillConfig.FrameCount)
            {
                isPlaying = false;
                skillConfig = null;
                if (rootMotionAction != null) animationController.ClearRootMotionAction();
                rootMotionAction = null;
                skillEndAction?.Invoke();
            }
        }
    }

    private void TickSkill()
    {
        currentFrameIndex += 1;
        //驱动动画
        if (animationController != null && skillConfig.SkillAnimationData.FrameDataDic.TryGetValue(currentFrameIndex, out SkillAnimationClipData skillAnimationEvent))
        {
            animationController.PlaySingleAniamtion(skillAnimationEvent.AnimationClip, 1, true, skillAnimationEvent.TransitionTime);

            if (skillAnimationEvent.ApplyRootMotion)
            {
                animationController.SetRootMotionAction(rootMotionAction);
            }
            else
            {
                animationController.ClearRootMotionAction();

            }
        }
        // 驱动音效
        for(int i = 0; i < skillConfig.SkillAudioData.FrameData.Count; i++)
        {
            SkillAudioEvent audioEvent = skillConfig.SkillAudioData.FrameData[i];
            if(audioEvent.audioClip != null && audioEvent.FrameIndex == currentFrameIndex)
            {
                // AudioManager.Instance.PlayerOneShot();
                PlayOneShot();
            }
        }

    }

    /// <summary>
    /// 中断技能、一般在状态机退出时使用
    /// </summary> 
    public void InterruptSkill()
    {
        isPlaying = false;
    }

    // 临时的技能音效播放器
    public void PlayOneShot()
    {
        Debug.Log("播放音乐");
    }
}
