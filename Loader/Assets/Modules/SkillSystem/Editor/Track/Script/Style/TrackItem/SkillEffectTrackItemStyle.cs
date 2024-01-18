using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillEffectTrackItemStyle : SkillTrackItemStyleBase
{
    private Label titleLabel;
    public VisualElement mainDragArea { get; private set; }
    // public VisualElement animationOverLine { get; private set; }
    private const string trackItemAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/TrackItem/AudioTrackItem.uxml";

    public bool isInit{get; private set;}
    public void Init(float frameUnitWidth, SkillEffectEvent skillEffectEvent, SkillMultiLineTrackStyle.ChildTrack childTrack)
    {   
        if(!isInit && skillEffectEvent.Prefab != null)
        {
            titleLabel = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackItemAssetPath).Instantiate().Query<Label>();//不要容器，直接持有目标物体
            root = titleLabel;
            mainDragArea = root.Q<VisualElement>("Main");
            childTrack.InitContent(root);

            isInit = true;
        }
    }
    
    public void ResetView(float frameUnitWidth, SkillEffectEvent skillEffectEvent)
    {
        if(!isInit) return;
        if(skillEffectEvent.Prefab != null)
        {
            SetTitle(skillEffectEvent.Prefab.name);

            ParticleSystem particleSystem = skillEffectEvent.Prefab.GetComponent<ParticleSystem>(); 

            SetWidth(frameUnitWidth * particleSystem.main.duration * SkillEditorWindows.Instance.SkillConfig.FrameRate);
            SetPosition(frameUnitWidth * skillEffectEvent.FrameIndex);
        }
        else
        {
            SetTitle("null");
            SetWidth(0);
            SetPosition(0);
        }

    }

    public void SetTitle(string title)
    {
        titleLabel.text = title;
    }
}
