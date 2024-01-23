using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectTrack : SkillTrackBase
{
 private SkillMultiLineTrackStyle trackStyle;
    public SkillEffectData EffectData { get => SkillEditorWindows.Instance.SkillConfig.SkillEffectData; }
    private List<EffectTrackItem> trackItemList = new List<EffectTrackItem>();
    public static Transform EffectParent{ get; private set; }
    public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        base.Init(menuParent, trackParent, frameWidth);
        trackStyle = new SkillMultiLineTrackStyle();
        trackStyle.Init(menuParent, trackParent, "特效配置", AddChildTrack, CheckDeleteChildTrack, SwapChildTrack, UpdateChildTrackName);

        if(SkillEditorWindows.Instance.OnEditorScene)
        {
            EffectParent = GameObject.Find("Effects").transform;
            EffectParent.position = Vector3.zero;
            EffectParent.rotation = Quaternion.identity;

            for(int i = EffectParent.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(EffectParent.GetChild(i).gameObject);
            }
        }
        ResetView();
    }



    public override void ResetView(float frameWidth)
    {   
        base.ResetView(frameWidth);

        // 销毁当前已有轨道
        foreach (EffectTrackItem track in trackItemList)
        {
            track.Destory();
        }
        trackItemList.Clear();

        foreach (SkillEffectEvent effectEvent in EffectData.FrameData)
        {
            CreateItem(effectEvent);
        }
    }
    public void CreateItem(SkillEffectEvent effectEvent)
    {
        EffectTrackItem effectTrackItem= new EffectTrackItem();
        
        effectTrackItem.Init(this, frameWidth, effectEvent, trackStyle.AddChildTrack());

        effectTrackItem.SetTrackName(effectEvent.TrackName);

        trackItemList.Add(effectTrackItem);
    }

    private void AddChildTrack()
    {
        SkillEffectEvent skillEffectEvent = new SkillEffectEvent();

        EffectData.FrameData.Add(skillEffectEvent);
        CreateItem(skillEffectEvent);

        SkillEditorWindows.Instance.SaveConfig();
    }

    private void UpdateChildTrackName(SkillMultiLineTrackStyle.ChildTrack childTrack, string newName)
    {
        // 同步给配置表
        EffectData.FrameData[childTrack.GetIndex()].TrackName = newName;
        SkillEditorWindows.Instance.SaveConfig();
    }

    private bool CheckDeleteChildTrack(int index)
    {
        if(index < 0 || index >= EffectData.FrameData.Count)
        {
            return false;
        }

        SkillEffectEvent skillEffectEvent = EffectData.FrameData[index];

        if(skillEffectEvent != null)
        {
            EffectData.FrameData.RemoveAt(index);

            SkillEditorWindows.Instance.SaveConfig();

            trackItemList[index].ClearEffectPreviewObj();

            trackItemList.RemoveAt(index);
        }

        return skillEffectEvent != null;
    }
    private void SwapChildTrack(int index1, int index2)
    {
        SkillEffectEvent data1 = EffectData.FrameData[index1];
        SkillEffectEvent data2 = EffectData.FrameData[index2];

        EffectData.FrameData[index1] = data2;
        EffectData.FrameData[index2] = data1;

        // 保存交给窗口的退出机制
    }
    public override void Destory()
    {   
        trackStyle.Destory();
        for(int i = 0; i < trackItemList.Count; i++)
        {
            trackItemList[i].ClearEffectPreviewObj(); 
        }
    }
    public override void TickView(int frameIndex)
    {   
        for(int i = 0; i < trackItemList.Count; i++)
        {
            trackItemList[i].TickView(frameIndex); 
        }
    }

    public override void OnStop()
    {

    }
}
