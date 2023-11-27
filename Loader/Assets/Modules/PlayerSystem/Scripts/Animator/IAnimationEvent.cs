using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationEvent
{
    public void OnMovementStateAnimationEnterEvent();
    public void OnMovementStateAnimationTransitionEvent();
    public void OnMovementStateAnimationExitEvent();
}
