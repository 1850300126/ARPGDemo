using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common;
using UnityEngine;
using UnityEngine.UIElements;
using static SkillMultiLineTrackStyle;

public class ParticleTrackItem : TrackItemBase<ParticleTrack>
{    
    // private SkillParticleData particleData;
    // public SkillParticleData ParticleData { get => particleData; }
    // private SkillParticleTrackItemStyle trackItemStyle;
    // // ����������Ч����֡��
    // public int frameRate = 60;
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="particleTrack">��ǰ�Ĺ��</param>
    // /// <param name="parentTrackStyle">��ǰ����ĸ���</param>
    // /// <param name="startFrameIndex">��ʼ֡</param>
    // /// <param name="frameUnitWidth">֡���</param>
    // /// <param name="particleData">����</param>
    // public void Init(ParticleTrack particleTrack, SkillTrackStyleBase parentTrackStyle, int startFrameIndex, float frameUnitWidth, SkillParticleData particleData)
    // {
    //     track = particleTrack;
    //     this.frameIndex = startFrameIndex;
    //     this.frameUnitWidth = frameUnitWidth;
    //     this.particleData = particleData;

    //     itemStyle = trackItemStyle = new SkillParticleTrackItemStyle();
    //     trackItemStyle.Init(parentTrackStyle, startFrameIndex, frameUnitWidth);

    //     normalColor = new Color(0.588f, 0.850f, 0.905f, 0.5f);
    //     selectColor = new Color(0.588f, 0.850f, 0.905f, 1f);
    //     OnUnSelect();

    //     //���¼�
    //     // trackItemStyle.mainDragArea.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    //     // trackItemStyle.mainDragArea.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
    //     // trackItemStyle.mainDragArea.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);
    //     // trackItemStyle.mainDragArea.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);

    //     ResetView(frameUnitWidth);
    // }    
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="particleTrack">��ǰ�Ĺ��</param>
    // /// <param name="parentTrackStyle">��ǰ����ĸ���</param>
    // /// <param name="startFrameIndex">��ʼ֡</param>
    // /// <param name="frameUnitWidth">֡���</param>
    // /// <param name="particleData">����</param>
    // public void Init(ParticleTrack particleTrack, ChildTrack childTrack, int startFrameIndex, float frameUnitWidth, SkillParticleData particleData)
    // {
    //     track = particleTrack;
    //     this.frameIndex = startFrameIndex;
    //     this.frameUnitWidth = frameUnitWidth;
    //     this.particleData = particleData;

    //     itemStyle = trackItemStyle = new SkillParticleTrackItemStyle();
    //     trackItemStyle.Init(childTrack, startFrameIndex, frameUnitWidth);

    //     normalColor = new Color(0.588f, 0.850f, 0.905f, 0.5f);
    //     selectColor = new Color(0.588f, 0.850f, 0.905f, 1f);
    //     OnUnSelect();

    //     //���¼�
    //     trackItemStyle.mainDragArea.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    //     trackItemStyle.mainDragArea.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
    //     trackItemStyle.mainDragArea.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);
    //     trackItemStyle.mainDragArea.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);

    //     ResetView(frameUnitWidth);
    // }    
    // public override void ResetView(float frameUnitWidth)
    // {
    //     base.ResetView(frameUnitWidth);

    //     this.frameUnitWidth = frameUnitWidth;
    //     // trackItemStyle.SetTitle(particleData.particlePrefab.name);

    //     //λ�ü���
    //     trackItemStyle.SetPosition(frameIndex * frameUnitWidth);
    //     trackItemStyle.SetWidth(particleData.DurationFrame * frameUnitWidth);

    //     //���㶯�������ߵ�λ��
    //     int animationClipFrameCount = (int)(particleData.particlePrefab.main.duration * frameRate);
    //     if (animationClipFrameCount > particleData.DurationFrame)
    //     {
    //         trackItemStyle.animationOverLine.style.display = DisplayStyle.None;
    //     }
    //     else
    //     {
    //         trackItemStyle.animationOverLine.style.display = DisplayStyle.Flex;
    //         Vector3 overLinePos = trackItemStyle.animationOverLine.transform.position;
    //         //overLinePos.x = animationClipFrameCount * frameUnitWidth - animationOverLine.style.width.value.value / 2;
    //         overLinePos.x = animationClipFrameCount * frameUnitWidth - 1; //�������Ϊ2��ȡһ��
    //         trackItemStyle.animationOverLine.transform.position = overLinePos;
    //     }
    // }

    // #region  ��꽻��
    // private bool mouseDrag = false;
    // private float startDragPosX;
    // private int startDragFrameIndex;

    // private void OnMouseDownEvent(MouseDownEvent evt)
    // {
    //     startDragPosX = evt.mousePosition.x;
    //     startDragFrameIndex = frameIndex;
    //     mouseDrag = true;

    //     Select();
    // }
    // private void OnMouseUpEvent(MouseUpEvent evt)
    // {
    //     if (mouseDrag) ApplyDrag();
    //     mouseDrag = false;
    // }

    // private void OnMouseOutEvent(MouseOutEvent evt)
    // {   
    //     if (mouseDrag) ApplyDrag();
    //     mouseDrag = false;
    // }
    // private void OnMouseMoveEvent(MouseMoveEvent evt)
    // {
    //     if (mouseDrag)
    //     {
    //         float offsetPos = evt.mousePosition.x - startDragPosX;
    //         int offsetFrame = Mathf.RoundToInt(offsetPos / frameUnitWidth);
    //         int targetFrameIndex = startDragFrameIndex + offsetFrame;

    //         if (targetFrameIndex < 0) return; //��������ק�����������

    //         //ȷ���޸ĵ�����
    //         frameIndex = targetFrameIndex;

    //         //��������Ҳ�߽磬��չ�߽�
    //         CheckFrameCount();

    //         //ˢ����ͼ
    //         ResetView(frameUnitWidth);
    //     }
    // }
    // /// <summary>
    // /// ��������Ҳ�߽磬��չ�߽�
    // /// </summary>
    // public void CheckFrameCount()
    // {
    //     if (frameIndex + particleData.DurationFrame > SkillEditorWindows.Instance.SkillConfig.FrameCount)
    //     {
    //         //�������õ��¶�����Ч����������
    //         SkillEditorWindows.Instance.CurrentFrameCount = frameIndex + particleData.DurationFrame;
    //     }
    // }

    // private void ApplyDrag()
    // {
    //     if (startDragFrameIndex != frameIndex)
    //     {
    //         track.SetFrameIndex(this, frameIndex);
    //         SkillEditorInspector.Instance.SetTrackItemFrameIndex(frameIndex);
    //     }
    // }
    // #endregion

    // public override void OnConfigChanged()
    // {   
    //     int index = track.trackItemDic[this].GetIndex();

    //     particleData = track.ParticleFrameData.skillParticleDatas[index];
        
    // }
}
