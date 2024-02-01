using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomEventTrack : SkillTrackBase
{
    private SkillMultiLineTrackStyle trackStyle;
    public SkillCustomData CustomEventData { get => SkillEditorWindows.Instance.SkillConfig.SkillCustomData; }
    private List<CustomEventTrackItem> trackItemList = new List<CustomEventTrackItem>();
    public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        base.Init(menuParent, trackParent, frameWidth);
        trackStyle = new SkillMultiLineTrackStyle();
        trackStyle.Init(menuParent, trackParent, "�Զ����¼�����", AddChildTrack, CheckDeleteChildTrack, SwapChildTrack, UpdateChildTrackName);

        ResetView();
    }

    public override void ResetView(float frameWidth)
    {   
        base.ResetView(frameWidth);

        // ���ٵ�ǰ���й��
        foreach (CustomEventTrackItem track in trackItemList)
        {
            track.Destory();
        }
        trackItemList.Clear();

        foreach (SkillCustomEvent effectEvent in CustomEventData.FrameData)
        {
            CreateItem(effectEvent);
        }
    }
    public void CreateItem(SkillCustomEvent effectEvent)
    {
        CustomEventTrackItem customEventTrackItem= new CustomEventTrackItem();
        
        customEventTrackItem.Init(this, frameWidth, effectEvent, trackStyle.AddChildTrack());

        customEventTrackItem.SetTrackName(effectEvent.TrackName);

        trackItemList.Add(customEventTrackItem);
    }

    private void AddChildTrack()
    {
        SkillCustomEvent skillCustomEvent = new SkillCustomEvent();

        CustomEventData.FrameData.Add(skillCustomEvent);
        CreateItem(skillCustomEvent);

        SkillEditorWindows.Instance.SaveConfig();
    }

    private void UpdateChildTrackName(SkillMultiLineTrackStyle.ChildTrack childTrack, string newName)
    {
        // ͬ�������ñ�
        CustomEventData.FrameData[childTrack.GetIndex()].TrackName = newName;
        SkillEditorWindows.Instance.SaveConfig();
    }

    private bool CheckDeleteChildTrack(int index)
    {
        if(index < 0 || index >= CustomEventData.FrameData.Count)
        {
            return false;
        }

        SkillCustomEvent skillCustomEvent = CustomEventData.FrameData[index];

        if(skillCustomEvent != null)
        {
            CustomEventData.FrameData.RemoveAt(index);

            SkillEditorWindows.Instance.SaveConfig();

            trackItemList.RemoveAt(index);
        }

        return skillCustomEvent != null;
    }
    private void SwapChildTrack(int index1, int index2)
    {
        SkillCustomEvent data1 = CustomEventData.FrameData[index1];
        SkillCustomEvent data2 = CustomEventData.FrameData[index2];

        CustomEventData.FrameData[index1] = data2;
        CustomEventData.FrameData[index2] = data1;

        // ���潻�����ڵ��˳�����
    }    
    public override void Destory()
    {   
        trackStyle.Destory();
    }
}
