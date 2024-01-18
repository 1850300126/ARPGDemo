using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioTrack : SkillTrackBase
{
    private SkillMultiLineTrackStyle trackStyle;
    public SkillAudioData AudioData { get => SkillEditorWindows.Instance.SkillConfig.SkillAudioData; }
    private List<AudioTrackItem> trackItemList = new List<AudioTrackItem>();
    public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        base.Init(menuParent, trackParent, frameWidth);
        trackStyle = new SkillMultiLineTrackStyle();
        trackStyle.Init(menuParent, trackParent, "音效配置", AddChildTrack, CheckDeleteChildTrack, SwapChildTrack, UpdateChildTrackName);

        ResetView();
    }



    public override void ResetView(float frameWidth)
    {   
        base.ResetView(frameWidth);

        // 销毁当前已有轨道
        foreach (AudioTrackItem track in trackItemList)
        {
            track.Destory();
        }
        trackItemList.Clear();

        if (SkillEditorWindows.Instance.SkillConfig == null) return;

        foreach (SkillAudioEvent audioEvent in AudioData.FrameData)
        {
            CreateItem(audioEvent);
        }
    }
    public void CreateItem(SkillAudioEvent audioEvent)
    {
        AudioTrackItem audioTrackItem= new AudioTrackItem();
        
        audioTrackItem.Init(this, frameWidth, audioEvent, trackStyle.AddChildTrack());

        audioTrackItem.SetTrackName(audioEvent.TrackName);

        trackItemList.Add(audioTrackItem);
    }

    private void AddChildTrack()
    {
        SkillAudioEvent skillAudioEvent = new SkillAudioEvent();

        AudioData.FrameData.Add(skillAudioEvent);
        CreateItem(skillAudioEvent);

        SkillEditorWindows.Instance.SaveConfig();
    }

    private void UpdateChildTrackName(SkillMultiLineTrackStyle.ChildTrack childTrack, string newName)
    {
        // 同步给配置表
        AudioData.FrameData[childTrack.GetIndex()].TrackName = newName;
        SkillEditorWindows.Instance.SaveConfig();
    }

    private bool CheckDeleteChildTrack(int index)
    {
        if(index < 0 || index >= AudioData.FrameData.Count)
        {
            return false;
        }

        SkillAudioEvent skillAudioEvent = AudioData.FrameData[index];

        if(skillAudioEvent != null)
        {
            AudioData.FrameData.RemoveAt(index);

            SkillEditorWindows.Instance.SaveConfig();
        }

        return skillAudioEvent != null;
    }
    private void SwapChildTrack(int index1, int index2)
    {
        SkillAudioEvent data1 = AudioData.FrameData[index1];
        SkillAudioEvent data2 = AudioData.FrameData[index2];

        AudioData.FrameData[index1] = data2;
        AudioData.FrameData[index2] = data1;

        // 保存交给窗口的退出机制
    }
    public override void Destory()
    {
        trackStyle.Destory();
    }

    public override void OnPlay(int startFrameIndex)
    {
        for(int i = 0; i < AudioData.FrameData.Count; i++)
        {
            SkillAudioEvent audioEvent = AudioData.FrameData[i];
            if(audioEvent.audioClip == null) continue;

            int audioFrameCount = (int)(audioEvent.audioClip.length * SkillEditorWindows.Instance.SkillConfig.FrameRate);
            int audioLastFrameIndex =  audioFrameCount + audioEvent.FrameIndex;
            // 时间轴在音频切片播放长度之内
            if(audioEvent.FrameIndex < startFrameIndex && audioLastFrameIndex > startFrameIndex)
            {
                int offset = startFrameIndex - audioEvent.FrameIndex;
                float playRate = (float)offset / audioFrameCount;
                EditorAudioUnility.PlayAudio(audioEvent.audioClip, playRate);
            }
            else if(audioEvent.FrameIndex == startFrameIndex)
            {
                EditorAudioUnility.PlayAudio(audioEvent.audioClip, 0);
            }
        }
    }

    public override void TickView(int frameIndex)
    {   
        if(SkillEditorWindows.Instance.IsPlaying)
        {
            for(int i = 0; i < AudioData.FrameData.Count; i++)
            {
                SkillAudioEvent audioEvent = AudioData.FrameData[i];
                if(audioEvent.audioClip != null && audioEvent.FrameIndex == frameIndex)
                {
                    // 从头播放
                    EditorAudioUnility.PlayAudio(audioEvent.audioClip, 0);
                }

            }
        }

    }

    public override void OnStop()
    {

    }


}
