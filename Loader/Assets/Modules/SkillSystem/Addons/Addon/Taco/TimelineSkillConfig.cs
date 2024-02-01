using System.Collections.Generic;
using Taco.Timeline;
using UnityEngine;

[CreateAssetMenu(fileName = "TimelineSkillConfig", menuName = "SkillSystem/TimelineSkillConfig", order = 0)]
public class TimelineSkillConfig : ScriptableObject 
{
    public List<Timeline> timelines = new List<Timeline>();
}
