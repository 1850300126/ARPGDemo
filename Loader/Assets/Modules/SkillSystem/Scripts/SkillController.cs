using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private AnimationController animationController;

    private bool isPlaying = false;     //��ǰ�Ƿ��ڲ���״̬
    public bool IsPlaying { get => isPlaying; }


    private SkillConfig skillConfig;    //��ǰ���ŵļ�������
    private int currentFrameIndex;      //��ǰ�ǵڼ�֡
    private float playTotalTime;        //��ǰ���ŵ���ʱ��
    private int frameRate;              //��ǰ���ܵ�֡��

    public void Init(AnimationController animationController)
    {
        this.animationController = animationController;
    }

    private Action<Vector3, Quaternion> rootMotionAction;
    private Action skillEndAction;

    /// <summary>
    /// ���ż���
    /// </summary>
    /// <param name="skillConfig"> �������� </param>
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
            //������ʱ���жϵ�ǰ�ǵڼ�֡
            int targetFrameIndex = (int)(playTotalTime * frameRate);
            //��ֹһ֡�ӳٹ���׷֡
            while (currentFrameIndex < targetFrameIndex)
            {
                //����һ�μ���
                TickSkill();
            }

            //����ﵽ���һ֡�����ܽ���
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
        //��������
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
        // ������Ч
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
    /// �жϼ��ܡ�һ����״̬���˳�ʱʹ��
    /// </summary> 
    public void InterruptSkill()
    {
        isPlaying = false;
    }

    // ��ʱ�ļ�����Ч������
    public void PlayOneShot()
    {
        Debug.Log("��������");
    }
}
