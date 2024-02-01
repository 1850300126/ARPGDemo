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
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    [DictionaryDrawerSettings(KeyLabel = "������", ValueLabel = "����")]
    public Dictionary<string, Action> customAcitons = new Dictionary<string, Action>();
    public PlaySkillEvent()
    {

    }
    public override void EventStart()
    {
        
    }
}
