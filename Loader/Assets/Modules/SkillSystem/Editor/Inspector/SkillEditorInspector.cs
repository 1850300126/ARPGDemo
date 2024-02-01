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
        else if (itemType == typeof(EffectTrackItem))
        {
            DrawEffectTrackItem((EffectTrackItem)currentTrackItem);
        }
        else if (itemType == typeof(EffectTrackItem))
        {
            DrawCustomEventTrackItem((CustomEventTrackItem)currentTrackItem);
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

    #region ��Ч���
    private FloatField effectDurationFiled;
    private void DrawEffectTrackItem(EffectTrackItem trackItem)
    {
        //������Դ
        ObjectField effectPrefabAssetField = new ObjectField("��ЧԤ����");
        effectPrefabAssetField.objectType = typeof(GameObject);
        effectPrefabAssetField.value = trackItem.SkillEffectEvent.Prefab;
        effectPrefabAssetField.RegisterValueChangedCallback(EffectPrefabValueChanged);
        root.Add(effectPrefabAssetField);

        // ���� 
        Vector3Field posFiled = new Vector3Field("����");
        posFiled.value = trackItem.SkillEffectEvent.Position;
        posFiled.RegisterValueChangedCallback(EffectPosFiledValueChanged);
        root.Add(posFiled);

        // ��ת
        Vector3Field rotFiled = new Vector3Field("��ת");
        rotFiled.value = trackItem.SkillEffectEvent.Rotation;
        rotFiled.RegisterValueChangedCallback(EffectRotFiledValueChanged);
        root.Add(rotFiled);

        // ��ת
        Vector3Field scaleFiled = new Vector3Field("����");
        scaleFiled.value = trackItem.SkillEffectEvent.Scale;
        scaleFiled.RegisterValueChangedCallback(EffectScaleFiledValueChanged);
        root.Add(scaleFiled);

        // �Զ�����
        Toggle autoDestructToggle = new Toggle("�Զ�����");
        autoDestructToggle.value = trackItem.SkillEffectEvent.AutoDestruct;
        autoDestructToggle.RegisterValueChangedCallback(EffectAutoDestructToggleValueChanged);
        root.Add(autoDestructToggle);
        // ʱ��
        
        effectDurationFiled = new FloatField("����ʱ��");
        effectDurationFiled.value = trackItem.SkillEffectEvent.Duration;
        effectDurationFiled.RegisterCallback<FocusInEvent>(EffectDurationFieldFocusIn);
        effectDurationFiled.RegisterCallback<FocusOutEvent>(EffectDurationFieldFocusOut);
        root.Add(effectDurationFiled);

        // ʱ����㰴ť
        Button calculateEffectButton = new Button(CalculateEffectDuration);
        calculateEffectButton.text = "���¼���ʱ��";
        root.Add(calculateEffectButton);

        // ����ģ��Transform��Ϣ
        Button applyModelTransformDataButton = new Button(ApplyModelTransformData);
        applyModelTransformDataButton.text = "����ģ��Transform��Ϣ";
        root.Add(applyModelTransformDataButton);
    }

    private void ApplyModelTransformData()
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        effectTrackItem.ApplyModelTransformData();
        Show();
    }

    private void CalculateEffectDuration()
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        ParticleSystem[] particleSystems = effectTrackItem.SkillEffectEvent.Prefab.GetComponentsInChildren<ParticleSystem>();
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

        effectTrackItem.SkillEffectEvent.Duration = particleSystems[current].main.duration;

        effectDurationFiled.value = effectTrackItem.SkillEffectEvent.Duration;

        effectTrackItem.ResetView();
    }


    private void EffectPrefabValueChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        EffectTrackItem effect = (EffectTrackItem)currentTrackItem;
        effect.SkillEffectEvent.Prefab = evt.newValue as GameObject;
        // ���¼�ʱ
        CalculateEffectDuration();
        effect.ResetView();
    }
    private void EffectPosFiledValueChanged(ChangeEvent<Vector3> evt)
    {
        EffectTrackItem effect = (EffectTrackItem)currentTrackItem;
        effect.SkillEffectEvent.Position = evt.newValue;
        effect.ResetView();

    }    
    private void EffectRotFiledValueChanged(ChangeEvent<Vector3> evt)
    {
        EffectTrackItem effect = (EffectTrackItem)currentTrackItem;
        effect.SkillEffectEvent.Rotation = evt.newValue;        
        effect.ResetView();

    }
    private void EffectScaleFiledValueChanged(ChangeEvent<Vector3> evt)
    {
        EffectTrackItem effect = (EffectTrackItem)currentTrackItem;
        effect.SkillEffectEvent.Scale = evt.newValue;        
        effect.ResetView();

    }
    private void EffectAutoDestructToggleValueChanged(ChangeEvent<bool> evt)
    {
        EffectTrackItem effect = (EffectTrackItem)currentTrackItem;
        effect.SkillEffectEvent.AutoDestruct = evt.newValue;
        effect.ResetView();
    }

    float oldEffectDurationField;
    private void EffectDurationFieldFocusIn(FocusInEvent evt)
    {
        oldEffectDurationField = effectDurationFiled.value;
    }    
    private void EffectDurationFieldFocusOut(FocusOutEvent evt)
    {
        if(effectDurationFiled.value != oldEffectDurationField)
        {   
            EffectTrackItem effect = (EffectTrackItem)currentTrackItem;
            effect.SkillEffectEvent.Duration = effectDurationFiled.value;
            effect.ResetView();
        }
    }
    #endregion

    # region �Զ���ű����
    private void DrawCustomEventTrackItem(CustomEventTrackItem trackItem)
    {

    }
    #endregion
}
