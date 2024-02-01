using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PlaySkillEvent", fileName = "PlaySkillEvent")]
public class PlaySkillEvent : CustomEventBase
{       
    [NonSerialized, OdinSerialize]//不通过Unity的序列化，用Odin 的序列化
    [DictionaryDrawerSettings(KeyLabel = "方法名", ValueLabel = "方法")]
    public Dictionary<string, Action> customAcitons = new Dictionary<string, Action>();
    public PlaySkillEvent()
    {

    }
    public override void EventStart()
    {
        
    }
}
