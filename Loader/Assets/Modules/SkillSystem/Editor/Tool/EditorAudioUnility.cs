using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
/// <summary>
/// ʹ�÷����õ�Editor���ɷ��ʵķ���
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
    /// ������Ч(Ԥ��״̬����Ҫѭ��)
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="start">0-1Ϊ���Ž���, �����10000��</param>
    public static void PlayAudio(AudioClip clip, float start)
    {
        playClipMehthodInfo.Invoke(clip, new object[] {clip, (int)(start * clip.frequency), false});
    }
    public static void StopAudio()
    {
        stopClipMehthodInfo.Invoke(null, null);
    }
}
