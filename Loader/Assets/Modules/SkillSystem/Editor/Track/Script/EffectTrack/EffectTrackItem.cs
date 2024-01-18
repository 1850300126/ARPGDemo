using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectTrackItem : TrackItemBase<EffectTrack>
{
    private SkillMultiLineTrackStyle.ChildTrack childTrackStyle;
    private SkillEffectTrackItemStyle trackItemStyle;
    private SkillEffectEvent skillEffectEvent;
    public SkillEffectEvent SkillEffectEvent{ get => skillEffectEvent;}
    public void Init(EffectTrack track,float frameUnitWidth, SkillEffectEvent skillEffectEvent, SkillMultiLineTrackStyle.ChildTrack childTrack)
    {
        this.track = track;
        this.frameIndex = skillEffectEvent.FrameIndex;
        this.childTrackStyle = childTrack;

        this.skillEffectEvent = skillEffectEvent;
        normalColor = new Color(0.388f, 0.850f, 0.905f, 0.5f);
        selectColor = new Color(0.388f, 0.850f, 0.905f, 1f);
        trackItemStyle = new SkillEffectTrackItemStyle();

        itemStyle = trackItemStyle;

        childTrackStyle.trackRoot.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
        childTrackStyle.trackRoot.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);
        ResetView(frameUnitWidth);
    }

    public override void ResetView(float frameUnitWidth)
    {
        base.ResetView(frameUnitWidth);
        if(skillEffectEvent.Prefab != null)
        {
            if(!trackItemStyle.isInit)
            {
                trackItemStyle.Init(frameUnitWidth, skillEffectEvent, childTrackStyle);
                //绑定事件
                trackItemStyle.mainDragArea.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
            }
        }
        trackItemStyle.ResetView(frameUnitWidth, skillEffectEvent);
    }

    public void Destory()
    {
        childTrackStyle.Destory();  
    }

    public void SetTrackName(string name)
    {
        childTrackStyle.SetTrackName(name);
    }


    # region 鼠标交互

    private bool mouseDrag = false;
    private float startDragPosX;
    private int startDragFrameIndex;

    private void OnMouseDownEvent(MouseDownEvent evt)
    {
        startDragPosX = evt.mousePosition.x;
        startDragFrameIndex = frameIndex;
        mouseDrag = true;

        Select();
    }

    private void OnMouseUpEvent(MouseUpEvent evt)
    {
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void OnMouseOutEvent(MouseOutEvent evt)
    {
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void OnMouseMoveEvent(MouseMoveEvent evt)
    {
        if (mouseDrag)
        {
            float offsetPos = evt.mousePosition.x - startDragPosX;
            int offsetFrame = Mathf.RoundToInt(offsetPos / frameUnitWidth);
            int targetFrameIndex = startDragFrameIndex + offsetFrame;
        
            if (targetFrameIndex < 0 || offsetFrame == 0) return; //不考虑拖拽到负数的情况

            //确定修改的数据
            frameIndex = targetFrameIndex;

            skillEffectEvent.FrameIndex = frameIndex;
            //如果超过右侧边界，拓展边界
            CheckFrameCount();

            //刷新视图
            ResetView(frameUnitWidth);
            
        }
    }

    /// <summary>
    /// 如果超过右侧边界，拓展边界
    /// </summary>
    public void CheckFrameCount()
    {   
        // int frameCount = (int)skillAudioEvent.audioClip.length * SkillEditorWindows.Instance.SkillConfig.FrameRate;
        // if (frameIndex + frameCount > SkillEditorWindows.Instance.SkillConfig.FrameCount)
        // {
        //     //保存配置导致对象无效，重新引用
        //     SkillEditorWindows.Instance.CurrentFrameCount = frameIndex + frameCount;
        // }
    }

    private void ApplyDrag()
    {
        if (startDragFrameIndex != frameIndex)
        {
            // skillAudioEvent.FrameIndex = frameIndex;

            // TODO:inspector 数据刷新
            // SkillEditorInspector.Instance.SetTrackItemFrameIndex(frameIndex);
        }
    }
    # endregion

    #region  拖拽资源
    private void OnDragUpdatedEvent(DragUpdatedEvent evt)
    {
        // UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        // AudioClip clip = objs[0] as AudioClip;
        // if (clip != null)
        // {
        //     DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        // }
    }

    private void OnDragExitedEvent(DragExitedEvent evt)
    {
        // UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        // AudioClip clip = objs[0] as AudioClip;
        // if (clip != null)
        // {
        //     //放置动画资源

        //     //当前选中的帧数位置 检测是否能放置动画
        //     int selectFrameIndex = SkillEditorWindows.Instance.GetFrameIndexByPos(evt.localMousePosition.x);
        //     if(selectFrameIndex >= 0)
        //     {
        //         // 构建默认音效数据
        //         // skillAudioEvent.audioClip = clip;
        //         // skillAudioEvent.FrameIndex = selectFrameIndex;
        //         // skillAudioEvent.Voluem = 1;

        //         // this.frameIndex = selectFrameIndex;
        //         // ResetView();
        //         // SkillEditorWindows.Instance.SaveChanges();
        //     }
            
        // }
    }

    #endregion
}
