using System;
using System.Collections;
using System.Collections.Generic;
using Taco.Timeline;
using UnityEngine;

public class AnimationController : MonoBehaviour
{   
    public Animator animator;

    public void Init()
    {
        animator = GetComponent<Animator>();
    }
    
    #region RootMotion
    private Action<Vector3, Quaternion> rootMotionAction;
    private void OnAnimatorMove()
    {   
        rootMotionAction?.Invoke(animator.deltaPosition, animator.deltaRotation);
    }
    public void SetRootMotionAction(Action<Vector3, Quaternion> rootMotionAction)
    {
        this.rootMotionAction = rootMotionAction;
    }
    public void ClearRootMotionAction()
    {
        rootMotionAction = null;
    }
    #endregion

    #region �����¼�
    private Dictionary<string, Action> eventDic = new Dictionary<string, Action>();
    // Animator�ᴥ����ʵ���¼�����
    private void AniamtionEvent(string eventName)
    {
        if (eventDic.TryGetValue(eventName, out Action action))
        {
            action?.Invoke();
        }
    }
    public void AddAniamtionEvent(string eventName, Action action)
    {
        if (eventDic.TryGetValue(eventName, out Action _action))
        {
            _action += action;
        }
        else
        {
            eventDic.Add(eventName, action);
        }
    }

    public void RemoveAnimationEvent(string eventName)
    {
        eventDic.Remove(eventName);
    }

    public void RemoveAnimationEvent(string eventName, Action action)
    {
        if (eventDic.TryGetValue(eventName, out Action _action))
        {
            _action -= action;
        }
    }

    public void CleanAllActionEvent()
    {
        eventDic.Clear();
    }
    #endregion
}
