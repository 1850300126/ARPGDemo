using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

// 轨道颜色
[TrackColor(255/255f, 255/255f, 255/255f)]
// 对应的节点类
[TrackClipType(typeof(NewPlayableAsset))]
// 绑定的类型
[TrackBindingType(typeof(GameObject))]
public class NewTrack : TrackAsset
{
    
}
