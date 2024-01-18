using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillAudioTrackItemStyle : SkillTrackItemStyleBase
{
    private Label titleLabel;
    public VisualElement mainDragArea { get; private set; }
    // public VisualElement animationOverLine { get; private set; }
    private const string trackItemAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/TrackItem/AudioTrackItem.uxml";

    public bool isInit{get; private set;}
    public void Init(float frameUnitWidth, SkillAudioEvent skillAudioEvent, SkillMultiLineTrackStyle.ChildTrack childTrack)
    {   
        if(!isInit && skillAudioEvent.audioClip != null)
        {
            titleLabel = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackItemAssetPath).Instantiate().Query<Label>();//不要容器，直接持有目标物体
            root = titleLabel;
            mainDragArea = root.Q<VisualElement>("Main");
            childTrack.InitContent(root);

            isInit = true;
        }
    }
    
    public void ResetView(float frameUnitWidth, SkillAudioEvent skillAudioEvent)
    {
        if(!isInit) return;
        if(skillAudioEvent.audioClip != null)
        {
            SetTitle(skillAudioEvent.audioClip.name);
            SetWidth(frameUnitWidth * skillAudioEvent.audioClip.length * SkillEditorWindows.Instance.SkillConfig.FrameRate);
            SetPosition(frameUnitWidth * skillAudioEvent.FrameIndex);
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
