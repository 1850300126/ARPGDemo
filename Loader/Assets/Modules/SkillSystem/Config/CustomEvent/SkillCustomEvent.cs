using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public class SkillCustomEvent
{
#if UNITY_EDITOR
    public string TrackName = "ÊÂ¼þ¹ìµÀ";
#endif
    public int FrameIndex = -1;
    public CustomEventBase CustomEvent;
    public void TriggerEvent()
    {
        CustomEvent.EventStart();
    }
}
