using System;
using System.Collections;
using System.Collections.Generic;
using Taco.Timeline;
using UnityEngine;

public class SkillController : TimelinePlayer
{
    public void PlayTimeline(Timeline timeline, Action value)
    { 
        AddTimeline(timeline);
        timeline.OnDone += () => 
        {
            value?.Invoke();
            RemoveTimeline(timeline);
            Destroy(timeline);
        };
    }
    public void StopTimeline(Timeline timeline)
    {   
        RemoveTimeline(timeline);
        Destroy(timeline);
    }
}
