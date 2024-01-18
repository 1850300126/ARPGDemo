using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static SkillMultiLineTrackStyle;

public class ParticleTrack : SkillTrackBase
{
//     private SkillMultiLineTrackStyle trackStyle;
//     // 整个轨道的数据
//     public SkillParticleFrameData ParticleFrameData { get => SkillEditorWindows.Instance.SkillConfig.SkillParticleData; }
//     // 缓存item和轨道的关系
//     public Dictionary<ParticleTrackItem, ChildTrack> trackItemDic = new Dictionary<ParticleTrackItem, ChildTrack>();
//     // 限制粒子特效播放帧率
//     public int frameRate = 60;
//     public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
//     {
//         base.Init(menuParent, trackParent, frameWidth);
//         trackStyle = new SkillMultiLineTrackStyle();
//         // trackStyle.Init(menuParent, trackParent, "特效配置", ChectAddChildTrack, CheckDeleteChildTrack);

        

//         trackStyle.contentRoot.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
//         trackStyle.contentRoot.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);

//         ResetView();
//     }
//     public override void ResetView(float frameWidth)
//     {
//         base.ResetView(frameWidth);

//         // 解除所有子轨道和item的绑定
//         foreach (var item in trackItemDic)
//         {
//             item.Value.DeleteItem(item.Key.itemStyle.root);
//             trackStyle.ClearChildTruck(item.Value);
//         }
//         // 清除字典和列表
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
//         // 将创建的truck缓存起来
//         trackItemDic.Add(trackItem, newChildTrack);
//     }
//     #region  拖拽资源
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

//         // 获取当前拖入的位置所对应的帧
//         int selectFrameIndex = SkillEditorWindows.Instance.GetFrameIndexByPos(evt.localMousePosition.x);

//         // 粒子特效持续时间
//         int durationFrame;

//         int clipFrameCount = (int)(particle.main.duration * frameRate);

//         durationFrame = clipFrameCount;
        
//         //构建动画数据
//         SkillParticleData particleData = new SkillParticleData()
//         {
//             particlePrefab = particle,
//             DurationFrame = durationFrame,
//             StartFrame = selectFrameIndex
//         };

//         // 保存新增的动画数据到Config
//         ParticleFrameData.skillParticleDatas.Add(particleData);
//         SkillEditorWindows.Instance.SaveConfig();

//         // 轨道比item多，说明有空轨道
//         if(trackStyle.childTracksList.Count > trackItemDic.Count )
//         {
//             ParticleTrackItem trackItem = new ParticleTrackItem();
//             // 取第一个空轨道
//             trackItem.Init(this, trackStyle.childTracksList[trackItemDic.Count], selectFrameIndex, frameWidth, particleData);
//             // 将创建的truck缓存起来
//             trackItemDic.Add(trackItem, trackStyle.childTracksList[trackItemDic.Count] );

//             return;
//         }
//         //绘制一个Item
//         CreateChildItem(selectFrameIndex, particleData);
//     }
//     #endregion

//     #region 按钮方法
//     private bool ChectAddChildTrack()
//     {   
//         Debug.Log("创建子集");

//         return true;
//     }

//     private bool CheckDeleteChildTrack(int index)
//     {
//         Debug.Log("删除子集:" + index);

//         ParticleFrameData.skillParticleDatas.RemoveAt(index);

//         SkillEditorWindows.Instance.SaveConfig();

//         return true;
//     }
//     #endregion

//     public void SetFrameIndex(ParticleTrackItem currentItem, int newIndex)
//     {
//         // 修改数据
//         int dataIndex = trackItemDic[currentItem].GetIndex();    
//         ParticleFrameData.skillParticleDatas[dataIndex].StartFrame = newIndex;

//         SkillEditorWindows.Instance.SaveConfig();
//     }    
    
//     public override void DeleteTrackItem(int frameIndex)
//     {   
//         // TODO: 显示面板删除
//         SkillEditorWindows.Instance.SaveConfig();
//     }

//     public override void OnConfigChanged()
//     {
//         // TODO: 显示面板change
//     }

//     private List<GameObject> gameObjects= new List<GameObject>();
//     private List<ParticleSystem> particleSystems= new List<ParticleSystem>();
//     public override void TickView(int frameIndex)
//     {
//         Debug.Log("帧预览");
//         base.TickView(frameIndex);

//         for(int i = 0; i < ParticleFrameData.skillParticleDatas.Count; i++)
//         {
//             if(ParticleFrameData.skillParticleDatas[i].particlePrefab == null)
//             {
//                 Debug.LogWarning("这个轨道的Item没有Prefab" + i);
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
