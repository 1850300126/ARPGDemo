using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectTrack : SkillTrackBase
{
 private SkillMultiLineTrackStyle trackStyle;
    public SkillEffectData EffectData { get => SkillEditorWindows.Instance.SkillConfig.SkillEffectData; }
    private List<EffectTrackItem> trackItemList = new List<EffectTrackItem>();
    public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        base.Init(menuParent, trackParent, frameWidth);
        trackStyle = new SkillMultiLineTrackStyle();
        trackStyle.Init(menuParent, trackParent, "特效配置", AddChildTrack, CheckDeleteChildTrack, SwapChildTrack, UpdateChildTrackName);

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

        if (SkillEditorWindows.Instance.SkillConfig == null) return;

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
    }

    public override void OnPlay(int startFrameIndex)
    {
        // for(int i = 0; i < AudioData.FrameData.Count; i++)
        // {
        //     SkillAudioEvent audioEvent = AudioData.FrameData[i];
        //     if(audioEvent.audioClip == null) continue;

        //     int audioFrameCount = (int)(audioEvent.audioClip.length * SkillEditorWindows.Instance.SkillConfig.FrameRate);
        //     int audioLastFrameIndex =  audioFrameCount + audioEvent.FrameIndex;
        //     // 时间轴在音频切片播放长度之内
        //     if(audioEvent.FrameIndex < startFrameIndex && audioLastFrameIndex > startFrameIndex)
        //     {
        //         int offset = startFrameIndex - audioEvent.FrameIndex;
        //         float playRate = (float)offset / audioFrameCount;
        //         EditorAudioUnility.PlayAudio(audioEvent.audioClip, playRate);
        //     }
        //     else if(audioEvent.FrameIndex == startFrameIndex)
        //     {
        //         EditorAudioUnility.PlayAudio(audioEvent.audioClip, 0);
        //     }
        // }
    }

    public override void TickView(int frameIndex)
    {   
        // if(SkillEditorWindows.Instance.IsPlaying)
        // {
        //     for(int i = 0; i < AudioData.FrameData.Count; i++)
        //     {
        //         SkillAudioEvent audioEvent = AudioData.FrameData[i];
        //         if(audioEvent.audioClip != null && audioEvent.FrameIndex == frameIndex)
        //         {
        //             // 从头播放
        //             EditorAudioUnility.PlayAudio(audioEvent.audioClip, 0);
        //         }

        //     }
        // }

    }

    public override void OnStop()
    {

    }
}
