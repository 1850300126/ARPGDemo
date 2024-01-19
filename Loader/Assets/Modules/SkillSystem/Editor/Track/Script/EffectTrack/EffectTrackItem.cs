using System;
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

        // 强行生成预览
        ClearEffectPreviewObj();
        TickView(SkillEditorWindows.Instance.CurrentSelectFrameIndex);
    }

    public void Destory()
    {
        ClearEffectPreviewObj();
        childTrackStyle.Destory();  
    }

    public void ClearEffectPreviewObj()
    {
        if(effectPreviewObj != null)
        {
            GameObject.DestroyImmediate(effectPreviewObj);
            effectPreviewObj = null;
        }
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
            skillEffectEvent.FrameIndex = frameIndex;
            SkillEditorInspector.Instance.SetTrackItemFrameIndex(frameIndex);
        }
    }
    # endregion

    #region  拖拽资源
    private void OnDragUpdatedEvent(DragUpdatedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        GameObject prefab = objs[0] as GameObject;
        if (prefab != null)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
    }

    private void OnDragExitedEvent(DragExitedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        GameObject prefab = objs[0] as GameObject;
        if (prefab != null)
        {
            //放置动画资源

            //当前选中的帧数位置 检测是否能放置动画
            int selectFrameIndex = SkillEditorWindows.Instance.GetFrameIndexByPos(evt.localMousePosition.x);
            if(selectFrameIndex >= 0)
            {
                // 构建默认特效数据
                skillEffectEvent.FrameIndex = selectFrameIndex;
                skillEffectEvent.Prefab = prefab;
                skillEffectEvent.Position = Vector3.zero;
                skillEffectEvent.AutoDestruct = true;

                ParticleSystem[] particleSystems = prefab.GetComponentsInChildren<ParticleSystem>();
                float max = -1;
                int current = -1;
                for(int i = 0; i < particleSystems.Length;i++)
                {
                    if(particleSystems[i].main.duration > max)
                    {
                        max = particleSystems[i].main.duration; 
                        current = i;
                    }
                } 
                skillEffectEvent.Duration = particleSystems[current].main.duration;

                this.frameIndex = selectFrameIndex;
                ResetView();
                SkillEditorWindows.Instance.SaveChanges();
            }
            
        }
    }

    #endregion

    # region 预览
    private GameObject effectPreviewObj;
    public void TickView(int frameIndex)
    {
        if(skillEffectEvent.Prefab == null) return;
        // 是不是在播放范围内
        int durationFrame = (int)skillEffectEvent.Duration * SkillEditorWindows.Instance.SkillConfig.FrameRate;

        if(skillEffectEvent.FrameIndex <= frameIndex && skillEffectEvent.FrameIndex + durationFrame > frameIndex)
        {
            // 对比预制体
            if(effectPreviewObj != null && effectPreviewObj.name != skillEffectEvent.Prefab.name)
            {
                GameObject.DestroyImmediate(effectPreviewObj);
                effectPreviewObj = null;
            }

            if(effectPreviewObj == null)
            {
                Transform characterRoot = SkillEditorWindows.Instance.PreviewCharacterObj.transform;
                // 获取模拟坐标
                Vector3 rotPostion = SkillEditorWindows.Instance.GetPositionForRootMotion(skillEffectEvent.FrameIndex, true);
                Vector3 oldPos = characterRoot.position;
                // 把角色临时设置到播放坐标
                characterRoot.position = rotPostion;

                Vector3 pos = characterRoot.TransformPoint(skillEffectEvent.Position);
                Vector3 rot = characterRoot.eulerAngles + skillEffectEvent.Rotation;


                // 还原坐标
                characterRoot.position = oldPos;

                // 实例化
                effectPreviewObj = GameObject.Instantiate(skillEffectEvent.Prefab, pos, Quaternion.Euler(rot), null);
                effectPreviewObj.name = skillEffectEvent.Prefab.name;
            }
                
            // 粒子模拟
            ParticleSystem[] particleSystems = effectPreviewObj.GetComponentsInChildren<ParticleSystem>();
            for(int i = 0; i < particleSystems.Length;i++)
            {   
                // 得到模拟帧数
                int simulateFrame = frameIndex - skillEffectEvent.FrameIndex;
                // 1 / 30
                particleSystems[i].Simulate((float)simulateFrame / SkillEditorWindows.Instance.SkillConfig.FrameRate);
            }
        }
        else
        {
            ClearEffectPreviewObj();
        }
    }
    #endregion
}
