using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAudioEvent
{
#if UNITY_EDITOR
    public string TrackName = "��Ч���";
#endif
    public int FrameIndex = -1;
    public AudioClip audioClip;
    // ����
    public float Voluem = 1;
}
