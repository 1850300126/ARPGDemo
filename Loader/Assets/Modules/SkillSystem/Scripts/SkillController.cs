using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Custom.Animation;
public class SkillController : MonoBehaviour
{
    private AnimationController animationController;

    private bool isPlaying = false;     //��ǰ�Ƿ��ڲ���״̬
    public bool IsPlaying { get => isPlaying; }


    private SkillConfig skillConfig;    //��ǰ���ŵļ�������
    private int currentFrameIndex;      //��ǰ�ǵڼ�֡
    private float playTotalTime;        //��ǰ���ŵ���ʱ��
    private int frameRate;              //��ǰ���ܵ�֡��

    private Transform modelTransform;

    public void Init(AnimationController animationController, Transform modelTransform)
    {
        this.animationController = animationController;
        this.modelTransform = modelTransform;
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
        // ������Ч
        for(int i = 0; i < skillConfig.SkillEffectData.FrameData.Count; i++)
        {   
            
            SkillEffectEvent effectEvent = skillConfig.SkillEffectData.FrameData[i];
            if(effectEvent.Prefab != null && effectEvent.FrameIndex == currentFrameIndex)
            {
                // ʵ������Ч
                GameObject effectObj = PoolSystem.instance.GetGameObject(effectEvent.Prefab.name);
                if(effectObj == null)   
                {
                    effectObj = GameObject.Instantiate(effectEvent.Prefab); 
                    effectObj.name = effectEvent.Prefab.name;
                }
                effectObj.transform.position = modelTransform.TransformPoint(effectEvent.Position);
                effectObj.transform.rotation = Quaternion.Euler(modelTransform.eulerAngles + effectEvent.Rotation);
                effectObj.transform.localScale = effectEvent.Scale;
                if(effectEvent.AutoDestruct)
                {
                    StartCoroutine(AutoDestructEffectGameObject(effectEvent.Duration, effectObj));
                }
            }
        }    
        // �����Զ����¼�
        for(int i = 0; i < skillConfig.SkillCustomData.FrameData.Count; i++)
        {   
            SkillCustomEvent effectEvent = skillConfig.SkillCustomData.FrameData[i];

            if(currentFrameIndex == effectEvent.FrameIndex)
            {
                effectEvent.TriggerEvent();
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

    private IEnumerator AutoDestructEffectGameObject(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        PoolSystem.instance.PushGameObject(obj);
    }
}
