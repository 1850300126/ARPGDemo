using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
/// <summary>
/// 使用反射拿到Editor不可访问的方法
/// </summary>
public static class EditorAudioUnility
{
    private static MethodInfo playClipMehthodInfo;
    private static MethodInfo stopClipMehthodInfo;
    static  EditorAudioUnility()
    {
        Assembly editorAssembly = typeof(UnityEditor.AudioImporter).Assembly;

        Type utilClassType = editorAssembly.GetType("UnityEditor.AudioUtil");

        playClipMehthodInfo = utilClassType.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public, null,
                                                    new Type[] {typeof(AudioClip), typeof(int), typeof(bool)}, null);
        
        stopClipMehthodInfo = utilClassType.GetMethod("StopAllPreviewClips", BindingFlags.Static | BindingFlags.Public);
    }

    /// <summary>
    /// 播放音效(预览状态不需要循环)
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="start">0-1为播放进度, 被拆成10000份</param>
    public static void PlayAudio(AudioClip clip, float start)
    {
        playClipMehthodInfo.Invoke(clip, new object[] {clip, (int)(start * clip.frequency), false});
    }
    public static void StopAudio()
    {
        stopClipMehthodInfo.Invoke(null, null);
    }
}
