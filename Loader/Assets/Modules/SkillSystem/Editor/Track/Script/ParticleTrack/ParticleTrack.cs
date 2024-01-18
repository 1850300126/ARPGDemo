using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static SkillMultiLineTrackStyle;

public class ParticleTrack : SkillTrackBase
{
//     private SkillMultiLineTrackStyle trackStyle;
//     // �������������
//     public SkillParticleFrameData ParticleFrameData { get => SkillEditorWindows.Instance.SkillConfig.SkillParticleData; }
//     // ����item�͹���Ĺ�ϵ
//     public Dictionary<ParticleTrackItem, ChildTrack> trackItemDic = new Dictionary<ParticleTrackItem, ChildTrack>();
//     // ����������Ч����֡��
//     public int frameRate = 60;
//     public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
//     {
//         base.Init(menuParent, trackParent, frameWidth);
//         trackStyle = new SkillMultiLineTrackStyle();
//         // trackStyle.Init(menuParent, trackParent, "��Ч����", ChectAddChildTrack, CheckDeleteChildTrack);

        

//         trackStyle.contentRoot.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
//         trackStyle.contentRoot.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);

//         ResetView();
//     }
//     public override void ResetView(float frameWidth)
//     {
//         base.ResetView(frameWidth);

//         // ��������ӹ����item�İ�
//         foreach (var item in trackItemDic)
//         {
//             item.Value.DeleteItem(item.Key.itemStyle.root);
//             trackStyle.ClearChildTruck(item.Value);
//         }
//         // ����ֵ���б�
//         trackItemDic.Clear();

//         if (SkillEditorWindows.Instance.SkillConfig == null) return;

//         for (int i = 0; i < ParticleFrameData.skillParticleDatas.Count; i++)
//         {
//             CreateChildItem(ParticleFrameData.skillParticleDatas[i].StartFrame, ParticleFrameData.skillParticleDatas[i]);
//         }

//     }
//     private void CreateChildItem(int selectFrameIndex, SkillParticleData skillParticleEvent)
//     {
//         ParticleTrackItem trackItem = new ParticleTrackItem();
//         ChildTrack newChildTrack = trackStyle.AddChildTrack();    

//         trackItem.Init(this, newChildTrack, selectFrameIndex, frameWidth, skillParticleEvent);
//         // ��������truck��������
//         trackItemDic.Add(trackItem, newChildTrack);
//     }
//     #region  ��ק��Դ
//     private void OnDragUpdatedEvent(DragUpdatedEvent evt)
//     {
//         UnityEngine.Object[] objs = DragAndDrop.objectReferences;
//         GameObject obj = objs[0] as GameObject;

//         if (obj != null)
//         {   
//             if(!obj.GetComponent<ParticleSystem>()) return;
//             DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
//         }
//     }
//     private void OnDragExitedEvent(DragExitedEvent evt)
//     {
//         UnityEngine.Object[] objs = DragAndDrop.objectReferences;
//         GameObject obj = objs[0] as GameObject;

//         if (!obj) return;
        
//         ParticleSystem particle = obj.GetComponent<ParticleSystem>();

//         // ��ȡ��ǰ�����λ������Ӧ��֡
//         int selectFrameIndex = SkillEditorWindows.Instance.GetFrameIndexByPos(evt.localMousePosition.x);

//         // ������Ч����ʱ��
//         int durationFrame;

//         int clipFrameCount = (int)(particle.main.duration * frameRate);

//         durationFrame = clipFrameCount;
        
//         //������������
//         SkillParticleData particleData = new SkillParticleData()
//         {
//             particlePrefab = particle,
//             DurationFrame = durationFrame,
//             StartFrame = selectFrameIndex
//         };

//         // ���������Ķ������ݵ�Config
//         ParticleFrameData.skillParticleDatas.Add(particleData);
//         SkillEditorWindows.Instance.SaveConfig();

//         // �����item�࣬˵���пչ��
//         if(trackStyle.childTracksList.Count > trackItemDic.Count )
//         {
//             ParticleTrackItem trackItem = new ParticleTrackItem();
//             // ȡ��һ���չ��
//             trackItem.Init(this, trackStyle.childTracksList[trackItemDic.Count], selectFrameIndex, frameWidth, particleData);
//             // ��������truck��������
//             trackItemDic.Add(trackItem, trackStyle.childTracksList[trackItemDic.Count] );

//             return;
//         }
//         //����һ��Item
//         CreateChildItem(selectFrameIndex, particleData);
//     }
//     #endregion

//     #region ��ť����
//     private bool ChectAddChildTrack()
//     {   
//         Debug.Log("�����Ӽ�");

//         return true;
//     }

//     private bool CheckDeleteChildTrack(int index)
//     {
//         Debug.Log("ɾ���Ӽ�:" + index);

//         ParticleFrameData.skillParticleDatas.RemoveAt(index);

//         SkillEditorWindows.Instance.SaveConfig();

//         return true;
//     }
//     #endregion

//     public void SetFrameIndex(ParticleTrackItem currentItem, int newIndex)
//     {
//         // �޸�����
//         int dataIndex = trackItemDic[currentItem].GetIndex();    
//         ParticleFrameData.skillParticleDatas[dataIndex].StartFrame = newIndex;

//         SkillEditorWindows.Instance.SaveConfig();
//     }    
    
//     public override void DeleteTrackItem(int frameIndex)
//     {   
//         // TODO: ��ʾ���ɾ��
//         SkillEditorWindows.Instance.SaveConfig();
//     }

//     public override void OnConfigChanged()
//     {
//         // TODO: ��ʾ���change
//     }

//     private List<GameObject> gameObjects= new List<GameObject>();
//     private List<ParticleSystem> particleSystems= new List<ParticleSystem>();
//     public override void TickView(int frameIndex)
//     {
//         Debug.Log("֡Ԥ��");
//         base.TickView(frameIndex);

//         for(int i = 0; i < ParticleFrameData.skillParticleDatas.Count; i++)
//         {
//             if(ParticleFrameData.skillParticleDatas[i].particlePrefab == null)
//             {
//                 Debug.LogWarning("��������Itemû��Prefab" + i);
//             }
//             else
//             {
// //                 gameObjects[i] = Object.Instantiate(ParticleFrameData.skillParticleDatas[i].particlePrefab.gameObject);
// //                 gameObjects[i].gameObject.SetActive(true);
// //                 particleSystems[i] = gameObjects[i].GetComponent<ParticleSystem>();
// //                 if(gameObjects[i] != null)
// //                 {
// // #if UNITY_EDITOR
// //                     Object.DestroyImmediate(gameObjects[i]);
// // #else
// //                     Object.Destroy(gameObjects[i]);
// // #endif
// //                 }
//             }
//         }


// //         if (Prefab == null) return;
// //         if (Parent == null) return;
// //         if (_obj != null)
// //         {
// // #if UNITY_EDITOR
// //             Object.DestroyImmediate(_obj);
// // #else
// //             Object.Destroy(_obj);
// // #endif
// //         }

// //         _obj = Object.Instantiate(Prefab, Parent);
// //         _obj.gameObject.SetActive(true);
// //         _particleSystem = _obj.GetComponent<ParticleSystem>();

//     }
//     public override void Destory()
//     {
//         trackStyle.Destory();
//     }
}
