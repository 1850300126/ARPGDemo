using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomEventTrackItem : TrackItemBase<CustomEventTrack>
{
    private SkillMultiLineTrackStyle.ChildTrack childTrackStyle;
    private SkillCustomEventTrackItemStyle trackItemStyle;
    private SkillCustomEvent skillCustomEvent;
    public SkillCustomEvent SkillCustomEvent{ get => skillCustomEvent;}
    public void Init(CustomEventTrack track,float frameUnitWidth, SkillCustomEvent skillCustomEvent, SkillMultiLineTrackStyle.ChildTrack childTrack)
    {
        this.track = track;
        this.frameIndex = skillCustomEvent.FrameIndex;
        this.childTrackStyle = childTrack;

        this.skillCustomEvent = skillCustomEvent;
        // normalColor = new Color(160/255f, 255f/255f, 117/255f, 0.5f);
        // selectColor = new Color(160/255f, 255f/255f, 117/255f, 1f);
        normalColor = new Color(0.33f, 0.33f, 0.33f, 0.5f);
        selectColor = new Color(0f, 0f, 0f, 1f);
        trackItemStyle = new SkillCustomEventTrackItemStyle();

        itemStyle = trackItemStyle;

        childTrackStyle.trackRoot.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
        childTrackStyle.trackRoot.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);
        ResetView(frameUnitWidth);
    }

    public override void ResetView(float frameUnitWidth)
    {
        base.ResetView(frameUnitWidth);
        if(skillCustomEvent.CustomEvent != null)
        {
            if(!trackItemStyle.isInit)
            {
                trackItemStyle.Init(frameUnitWidth, skillCustomEvent, childTrackStyle);
                //绑定事件
                trackItemStyle.mainDragArea.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
            }
        }
        trackItemStyle.ResetView(frameUnitWidth, skillCustomEvent);
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

            skillCustomEvent.FrameIndex = frameIndex;
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
    }

    private void ApplyDrag()
    {
        if (startDragFrameIndex != frameIndex)
        {
            skillCustomEvent.FrameIndex = frameIndex;
            SkillEditorInspector.Instance.SetTrackItemFrameIndex(frameIndex);
        }
    }
    # endregion

    #region  拖拽资源
    private void OnDragUpdatedEvent(DragUpdatedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        CustomEventBase customEvent = objs[0] as CustomEventBase;
        if (customEvent != null)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
    }

    private void OnDragExitedEvent(DragExitedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        CustomEventBase customEvent = objs[0] as CustomEventBase;
        if (customEvent != null)
        {

            int selectFrameIndex = SkillEditorWindows.Instance.GetFrameIndexByPos(evt.localMousePosition.x);
            if(selectFrameIndex >= 0)
            {   
                skillCustomEvent.FrameIndex = selectFrameIndex;
                skillCustomEvent.CustomEvent = customEvent;

                this.frameIndex = selectFrameIndex;
                ResetView();
                SkillEditorWindows.Instance.SaveChanges();

            }
            
        }
    }

    #endregion
}
