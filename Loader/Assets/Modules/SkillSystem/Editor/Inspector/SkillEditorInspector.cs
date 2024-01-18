using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;

[CustomEditor(typeof(SkillEditorWindows))]
public class SkillEditorInspector : Editor
{
    public static SkillEditorInspector Instance;
    private static TrackItemBase currentTrackItem;
    private static SkillTrackBase currentTrack;

    private VisualElement root;


    public static void SetTrackItem(TrackItemBase trackItem, SkillTrackBase track)
    {
        if (currentTrackItem != null)
        {
            currentTrackItem.OnUnSelect();
        }

        currentTrackItem = trackItem;
        currentTrackItem.OnSelect();
        currentTrack = track;

        // 避免已经打开Inspector，导致的面板刷新不及时
        if (Instance != null) Instance.Show();
    }

    private void OnDestroy()
    {
        //说明窗口卸载
        if (currentTrackItem != null)
        {
            currentTrackItem.OnUnSelect();
            currentTrackItem = null;
            currentTrack = null;
        }
    }


    public override VisualElement CreateInspectorGUI()
    {   

        Instance = this;

        root = new VisualElement();

        Show();

        return root;
    }


    private void Show()
    {
        Clean();
        if (currentTrackItem == null) return;

        // TODO:补充其他类型
        Type itemType = currentTrackItem.GetType();

        if (itemType == typeof(AnimationTrackItem))
        {   
            DrawAnimationTrackItem((AnimationTrackItem)currentTrackItem);
        }
        else if (itemType == typeof(AudioTrackItem))
        {
            DrawAudioTrackItem((AudioTrackItem)currentTrackItem);
        }
    }


    private void Clean()
    {
        if (root != null)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                root.RemoveAt(i);
            }
        }
    }

    private int trackItemFrameIndex; //轨道对应的帧索引
    public void SetTrackItemFrameIndex(int trackItemFrameIndex)
    {
        this.trackItemFrameIndex = trackItemFrameIndex;
    }

    #region 动画轨道
    private Label clipFrameLabel;
    private Toggle rootMotionToggle;
    private Label isLoopLable;
    private IntegerField durationField;
    private FloatField transitionTimeField;

    private void DrawAnimationTrackItem(AnimationTrackItem animationTrackItem)
    {
        trackItemFrameIndex = animationTrackItem.FrameIndex;

        //动画资源
        ObjectField animationClipAssetField = new ObjectField("动画资源");
        animationClipAssetField.objectType = typeof(AnimationClip);
        animationClipAssetField.value = animationTrackItem.AnimationEvent.AnimationClip;
        animationClipAssetField.RegisterValueChangedCallback(AnimationClipValueChangedCallback);
        root.Add(animationClipAssetField);

        //根运动
        rootMotionToggle = new Toggle("应用根运动");
        rootMotionToggle.value = animationTrackItem.AnimationEvent.ApplyRootMotion;
        rootMotionToggle.RegisterValueChangedCallback(rootMotionToggleValueChanged);
        root.Add(rootMotionToggle);

        //轨道长度
        durationField = new IntegerField("轨道长度");
        durationField.value = animationTrackItem.AnimationEvent.DurationFrame;

        durationField.RegisterCallback<FocusInEvent>(DurtionFieldFocusIn);
        durationField.RegisterCallback<FocusOutEvent>(DurtionFieldFocusOut);

        // durationField.RegisterValueChangedCallback(DurtionFieldValueChangedCallback);
        root.Add(durationField);

        //过渡时间
        transitionTimeField = new FloatField("过渡时间");
        transitionTimeField.value = animationTrackItem.AnimationEvent.TransitionTime;    

        transitionTimeField.RegisterCallback<FocusInEvent>(TransitionTimeFieldFocusIn);
        transitionTimeField.RegisterCallback<FocusOutEvent>(TransitionTimeFieldFocusOut);

        // transitionTimeField.RegisterValueChangedCallback(TransitionTimeFieldValueChangedCallback);
        root.Add(transitionTimeField);

        //动画相关的信息
        int clipFrameCount = (int)(animationTrackItem.AnimationEvent.AnimationClip.length * animationTrackItem.AnimationEvent.AnimationClip.frameRate);
        clipFrameLabel = new Label("动画资源长度：" + clipFrameCount);
        root.Add(clipFrameLabel);

        isLoopLable = new Label("循环动画：" + animationTrackItem.AnimationEvent.AnimationClip.isLooping);
        root.Add(isLoopLable);

        //删除
        Button deleteButton = new Button(DeleteButtonClick);
        deleteButton.text = "删除";
        deleteButton.style.backgroundColor = new Color(1, 0, 0, 0.5f);
        root.Add(deleteButton);
    }

    private void AnimationClipValueChangedCallback(ChangeEvent<UnityEngine.Object> evt)
    {
        AnimationClip clip = evt.newValue as AnimationClip;

        //修改自身显示效果
        clipFrameLabel.text = "动画资源长度：" + ((int)(clip.length * clip.frameRate));
        isLoopLable.text = "循环动画：" + clip.isLooping;

        //保存到配置
        (currentTrackItem as AnimationTrackItem).AnimationEvent.AnimationClip = clip;
        SkillEditorWindows.Instance.SaveConfig();
        currentTrackItem.ResetView();
    }

    private void rootMotionToggleValueChanged(ChangeEvent<bool> evt)
    {
        (currentTrackItem as AnimationTrackItem).AnimationEvent.ApplyRootMotion = evt.newValue;
        SkillEditorWindows.Instance.SaveConfig();
    }

    int oldDurationValue;
    private void DurtionFieldFocusIn(FocusInEvent evt)
    {
        oldDurationValue = durationField.value;
    }    
    private void DurtionFieldFocusOut(FocusOutEvent evt)
    {
        if(durationField.value != oldDurationValue)
        {   
            //安全校验
            if (((AnimationTrack)currentTrack).CheckFrameIndexOnDrag(trackItemFrameIndex + durationField.value, trackItemFrameIndex, false))
            {
                //修改数据，刷新视图
                (currentTrackItem as AnimationTrackItem).AnimationEvent.DurationFrame = durationField.value;
                (currentTrackItem as AnimationTrackItem).CheckFrameCount();
                SkillEditorWindows.Instance.SaveConfig();//注意要最后保存，不然新旧数据会对不上
                currentTrackItem.ResetView();
            }
            else
            {
                durationField.value = oldDurationValue;
            }
        }
    }
    float oldTransitionTimeValue;
    private void TransitionTimeFieldFocusIn(FocusInEvent evt)
    {
        oldTransitionTimeValue = transitionTimeField.value;
    }    
    private void TransitionTimeFieldFocusOut(FocusOutEvent evt)
    {
        if(transitionTimeField.value != oldTransitionTimeValue)
        {   
            ((AnimationTrackItem)currentTrackItem).AnimationEvent.TransitionTime = transitionTimeField.value;
        }
    }
    private void DurtionFieldValueChangedCallback(ChangeEvent<int> evt)
    {
        int value = evt.newValue;

        //安全校验
        if ((currentTrack as AnimationTrack).CheckFrameIndexOnDrag(trackItemFrameIndex + value, trackItemFrameIndex, false))
        {
            //修改数据，刷新视图
            (currentTrackItem as AnimationTrackItem).AnimationEvent.DurationFrame = value;
            (currentTrackItem as AnimationTrackItem).CheckFrameCount();
            SkillEditorWindows.Instance.SaveConfig();//注意要最后保存，不然新旧数据会对不上
            currentTrackItem.ResetView();
        }
        else
        {
            durationField.value = evt.previousValue;
        }
    }

    private void TransitionTimeFieldValueChangedCallback(ChangeEvent<float> evt)
    {
        (currentTrackItem as AnimationTrackItem).AnimationEvent.TransitionTime = evt.newValue;
        SkillEditorWindows.Instance.SaveConfig();
        currentTrack.ResetView();
    }

    private void DeleteButtonClick()
    {
        currentTrack.DeleteTrackItem(trackItemFrameIndex); //此函数提供数据保存和刷新视图的逻辑
        Selection.activeObject = null;
    }

    #endregion

    #region 音效轨道

    private FloatField voluemFiled;
    private void DrawAudioTrackItem(AudioTrackItem trackItem)
    {
        //动画资源
        ObjectField audioClipAssetField = new ObjectField("动画资源");
        audioClipAssetField.objectType = typeof(AudioClip);
        audioClipAssetField.value = trackItem.SkillAudioEvent.audioClip;
        audioClipAssetField.RegisterValueChangedCallback(AudioClipValueChangedCallback);
        root.Add(audioClipAssetField);

        // 音量
        voluemFiled = new FloatField("播放音量");
        voluemFiled.value = trackItem.SkillAudioEvent.Voluem;
        voluemFiled.RegisterCallback<FocusInEvent>(VoluemFieldFocusIn);
        voluemFiled.RegisterCallback<FocusOutEvent>(VoluemFieldFocusOut);
        root.Add(voluemFiled);

    }

    private void AudioClipValueChangedCallback(ChangeEvent<UnityEngine.Object> evt)
    {
        AudioClip audioClip = evt.newValue as AudioClip; 
        ((AudioTrackItem)currentTrackItem).SkillAudioEvent.audioClip = audioClip;   
        currentTrackItem.ResetView();
    }
    float oldVoluemFieldValue;
    private void VoluemFieldFocusIn(FocusInEvent evt)
    {
        oldVoluemFieldValue = voluemFiled.value;
    }    
    private void VoluemFieldFocusOut(FocusOutEvent evt)
    {
        if(voluemFiled.value != oldVoluemFieldValue)
        {   
            ((AudioTrackItem)currentTrackItem).SkillAudioEvent.Voluem = voluemFiled.value;
        }
    }
    #endregion
}
