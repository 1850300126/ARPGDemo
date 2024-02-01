using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioTrackItem : TrackItemBase<AudioTrack>
{   
    private SkillMultiLineTrackStyle.ChildTrack childTrackStyle;
    private SkillAudioTrackItemStyle trackItemStyle;
    private SkillAudioEvent skillAudioEvent;
    public SkillAudioEvent SkillAudioEvent{ get => skillAudioEvent;}
    public void Init(AudioTrack track,float frameUnitWidth, SkillAudioEvent skillAudioEvent, SkillMultiLineTrackStyle.ChildTrack childTrack)
    {
        this.track = track;
        this.frameIndex = skillAudioEvent.FrameIndex;
        this.childTrackStyle = childTrack;

        this.skillAudioEvent = skillAudioEvent;
        // normalColor = new Color(1f, 191/255f, 7/255f, 0.5f);
        // selectColor = new Color(1f, 191/255f, 7/255f, 1f);
        normalColor = new Color(0.33f, 0.33f, 0.33f, 0.5f);
        selectColor = new Color(0f, 0f, 0f, 1f);
        trackItemStyle = new SkillAudioTrackItemStyle();

        itemStyle = trackItemStyle;

        childTrackStyle.trackRoot.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
        childTrackStyle.trackRoot.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);
        ResetView(frameUnitWidth);
    }

    public override void ResetView(float frameUnitWidth)
    {
        base.ResetView(frameUnitWidth);
        if(skillAudioEvent.audioClip != null)
        {
            if(!trackItemStyle.isInit)
            {
                trackItemStyle.Init(frameUnitWidth, skillAudioEvent, childTrackStyle);
                //���¼�
                trackItemStyle.mainDragArea.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);
                trackItemStyle.mainDragArea.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
            }
        }
        trackItemStyle.ResetView(frameUnitWidth, skillAudioEvent);
    }

    public void Destory()
    {
        childTrackStyle.Destory();  
    }

    public void SetTrackName(string name)
    {
        childTrackStyle.SetTrackName(name);
    }


    # region ��꽻��

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
        
            if (targetFrameIndex < 0 || offsetFrame == 0) return; //��������ק�����������

            //ȷ���޸ĵ�����
            frameIndex = targetFrameIndex;

            skillAudioEvent.FrameIndex = frameIndex;
            //��������Ҳ�߽磬��չ�߽�
            CheckFrameCount();

            //ˢ����ͼ
            ResetView(frameUnitWidth);
            
        }
    }

    /// <summary>
    /// ��������Ҳ�߽磬��չ�߽�
    /// </summary>
    public void CheckFrameCount()
    {   
        int frameCount = (int)skillAudioEvent.audioClip.length * SkillEditorWindows.Instance.SkillConfig.FrameRate;
        if (frameIndex + frameCount > SkillEditorWindows.Instance.SkillConfig.FrameCount)
        {
            //�������õ��¶�����Ч����������
            SkillEditorWindows.Instance.CurrentFrameCount = frameIndex + frameCount;
        }
    }

    private void ApplyDrag()
    {
        if (startDragFrameIndex != frameIndex)
        {
            skillAudioEvent.FrameIndex = frameIndex;
            SkillEditorInspector.Instance.SetTrackItemFrameIndex(frameIndex);
        }
    }
    # endregion

    #region  ��ק��Դ
    private void OnDragUpdatedEvent(DragUpdatedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AudioClip clip = objs[0] as AudioClip;
        if (clip != null)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
    }

    private void OnDragExitedEvent(DragExitedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AudioClip clip = objs[0] as AudioClip;
        if (clip != null)
        {
            //���ö�����Դ

            //��ǰѡ�е�֡��λ�� ����Ƿ��ܷ��ö���
            int selectFrameIndex = SkillEditorWindows.Instance.GetFrameIndexByPos(evt.localMousePosition.x);
            if(selectFrameIndex >= 0)
            {
                // ����Ĭ����Ч����
                skillAudioEvent.audioClip = clip;
                skillAudioEvent.FrameIndex = selectFrameIndex;
                skillAudioEvent.Voluem = 1;

                this.frameIndex = selectFrameIndex;
                ResetView();
                SkillEditorWindows.Instance.SaveChanges();
            }
          
        }
    }

    #endregion
}
