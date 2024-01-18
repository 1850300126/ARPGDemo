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

        // �����Ѿ���Inspector�����µ����ˢ�²���ʱ
        if (Instance != null) Instance.Show();
    }

    private void OnDestroy()
    {
        //˵������ж��
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

        // TODO:������������
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

    private int trackItemFrameIndex; //�����Ӧ��֡����
    public void SetTrackItemFrameIndex(int trackItemFrameIndex)
    {
        this.trackItemFrameIndex = trackItemFrameIndex;
    }

    #region �������
    private Label clipFrameLabel;
    private Toggle rootMotionToggle;
    private Label isLoopLable;
    private IntegerField durationField;
    private FloatField transitionTimeField;

    private void DrawAnimationTrackItem(AnimationTrackItem animationTrackItem)
    {
        trackItemFrameIndex = animationTrackItem.FrameIndex;

        //������Դ
        ObjectField animationClipAssetField = new ObjectField("������Դ");
        animationClipAssetField.objectType = typeof(AnimationClip);
        animationClipAssetField.value = animationTrackItem.AnimationEvent.AnimationClip;
        animationClipAssetField.RegisterValueChangedCallback(AnimationClipValueChangedCallback);
        root.Add(animationClipAssetField);

        //���˶�
        rootMotionToggle = new Toggle("Ӧ�ø��˶�");
        rootMotionToggle.value = animationTrackItem.AnimationEvent.ApplyRootMotion;
        rootMotionToggle.RegisterValueChangedCallback(rootMotionToggleValueChanged);
        root.Add(rootMotionToggle);

        //�������
        durationField = new IntegerField("�������");
        durationField.value = animationTrackItem.AnimationEvent.DurationFrame;

        durationField.RegisterCallback<FocusInEvent>(DurtionFieldFocusIn);
        durationField.RegisterCallback<FocusOutEvent>(DurtionFieldFocusOut);

        // durationField.RegisterValueChangedCallback(DurtionFieldValueChangedCallback);
        root.Add(durationField);

        //����ʱ��
        transitionTimeField = new FloatField("����ʱ��");
        transitionTimeField.value = animationTrackItem.AnimationEvent.TransitionTime;    

        transitionTimeField.RegisterCallback<FocusInEvent>(TransitionTimeFieldFocusIn);
        transitionTimeField.RegisterCallback<FocusOutEvent>(TransitionTimeFieldFocusOut);

        // transitionTimeField.RegisterValueChangedCallback(TransitionTimeFieldValueChangedCallback);
        root.Add(transitionTimeField);

        //������ص���Ϣ
        int clipFrameCount = (int)(animationTrackItem.AnimationEvent.AnimationClip.length * animationTrackItem.AnimationEvent.AnimationClip.frameRate);
        clipFrameLabel = new Label("������Դ���ȣ�" + clipFrameCount);
        root.Add(clipFrameLabel);

        isLoopLable = new Label("ѭ��������" + animationTrackItem.AnimationEvent.AnimationClip.isLooping);
        root.Add(isLoopLable);

        //ɾ��
        Button deleteButton = new Button(DeleteButtonClick);
        deleteButton.text = "ɾ��";
        deleteButton.style.backgroundColor = new Color(1, 0, 0, 0.5f);
        root.Add(deleteButton);
    }

    private void AnimationClipValueChangedCallback(ChangeEvent<UnityEngine.Object> evt)
    {
        AnimationClip clip = evt.newValue as AnimationClip;

        //�޸�������ʾЧ��
        clipFrameLabel.text = "������Դ���ȣ�" + ((int)(clip.length * clip.frameRate));
        isLoopLable.text = "ѭ��������" + clip.isLooping;

        //���浽����
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
            //��ȫУ��
            if (((AnimationTrack)currentTrack).CheckFrameIndexOnDrag(trackItemFrameIndex + durationField.value, trackItemFrameIndex, false))
            {
                //�޸����ݣ�ˢ����ͼ
                (currentTrackItem as AnimationTrackItem).AnimationEvent.DurationFrame = durationField.value;
                (currentTrackItem as AnimationTrackItem).CheckFrameCount();
                SkillEditorWindows.Instance.SaveConfig();//ע��Ҫ��󱣴棬��Ȼ�¾����ݻ�Բ���
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

        //��ȫУ��
        if ((currentTrack as AnimationTrack).CheckFrameIndexOnDrag(trackItemFrameIndex + value, trackItemFrameIndex, false))
        {
            //�޸����ݣ�ˢ����ͼ
            (currentTrackItem as AnimationTrackItem).AnimationEvent.DurationFrame = value;
            (currentTrackItem as AnimationTrackItem).CheckFrameCount();
            SkillEditorWindows.Instance.SaveConfig();//ע��Ҫ��󱣴棬��Ȼ�¾����ݻ�Բ���
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
        currentTrack.DeleteTrackItem(trackItemFrameIndex); //�˺����ṩ���ݱ����ˢ����ͼ���߼�
        Selection.activeObject = null;
    }

    #endregion

    #region ��Ч���

    private FloatField voluemFiled;
    private void DrawAudioTrackItem(AudioTrackItem trackItem)
    {
        //������Դ
        ObjectField audioClipAssetField = new ObjectField("������Դ");
        audioClipAssetField.objectType = typeof(AudioClip);
        audioClipAssetField.value = trackItem.SkillAudioEvent.audioClip;
        audioClipAssetField.RegisterValueChangedCallback(AudioClipValueChangedCallback);
        root.Add(audioClipAssetField);

        // ����
        voluemFiled = new FloatField("��������");
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
