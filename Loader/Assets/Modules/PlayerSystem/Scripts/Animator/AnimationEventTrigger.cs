using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    private IAnimationEvent animation_event_inter;

    private Animator animator;

    public void InitEventTrigger(IAnimationEvent inter)
    {
        animation_event_inter = inter;

        animator = this.GetComponent<Animator>();
    }

    public void TriggerOnMovementStateAnimationEnterEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }
        animation_event_inter.OnMovementStateAnimationEnterEvent();
    }

    public void TriggerOnMovementStateAnimationExitEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }
        animation_event_inter.OnMovementStateAnimationExitEvent();
    }

    public void TriggerOnMovementStateAnimationTransitionEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }
        animation_event_inter.OnMovementStateAnimationTransitionEvent();
    }
    private bool IsInAnimationTransition(int layer_index = 0)
    {
        return animator.IsInTransition(layer_index);
    }

    // private void OnAnimatorMove()
    // {
    //    	if (animator.GetCurrentAnimatorStateInfo(0).IsTag("RootMotion"))
    //     {   
    //         animator.applyRootMotion = true;
    //     }
    //     // 否则由其他代码控制
    //     else
    //     {
    //         animator.applyRootMotion = false;
    //     }

    //     Debug.Log(animator.applyRootMotion);
    // }
}
