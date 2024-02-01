using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillCustomEventTrackItemStyle : SkillTrackItemStyleBase
{

    private Label titleLabel;
    public VisualElement mainDragArea { get; private set; }
    // public VisualElement animationOverLine { get; private set; }
    private const string trackItemAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/TrackItem/Item/CustomEventTrackItem.uxml";

    public bool isInit{get; private set;}
    public void Init(float frameUnitWidth, SkillCustomEvent skillCustomEventEvent, SkillMultiLineTrackStyle.ChildTrack childTrack)
    {   
        if(!isInit && skillCustomEventEvent != null)
        {
            titleLabel = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackItemAssetPath).Instantiate().Query<Label>();//不要容器，直接持有目标物体
            root = titleLabel;
            mainDragArea = root.Q<VisualElement>("Main");
            childTrack.InitContent(root);

            isInit = true;
        }
    }
    
    public void ResetView(float frameUnitWidth, SkillCustomEvent skillCustomEvent)
    {
        if(!isInit) return;
        if(skillCustomEvent != null)
        {
            SetTitle(skillCustomEvent.TrackName);
            SetWidth(frameUnitWidth * 5);
            SetPosition(frameUnitWidth * skillCustomEvent.FrameIndex);
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
