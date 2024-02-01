using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Taco.Timeline
{
    [ScriptGuid("3f0d14cafa6f2c84389c42789ec00083"), IconGuid("e6435fa591ae4414eb0f26dc6410086e"), Ordered(0), Color(127, 253, 228)]
    public class AnimationTrack : Track
    {
        static AvatarMask s_FullBodyMask;
        public static AvatarMask FullBodyMask
        {
            get
            {
                if (s_FullBodyMask == null)
                {

                    s_FullBodyMask = new AvatarMask();
                    for (int i = 0; i < 13; i++)
                    {
                        s_FullBodyMask.SetHumanoidBodyPartActive((AvatarMaskBodyPart)i, true);
                    }
                }
                return s_FullBodyMask;
            }
        }

        [ShowInInspector, OnValueChanged("RebindTimeline")]
        public AvatarMask AvatarMask;

        public int PlayableIndex { get; protected set; }
        public TimelineAnimationTrackPlayable TrackPlayable { get; protected set; }
        public List<TimelineAnimationClipPlayable> ClipPlayables { get; protected set; }

        public event Action Delay;


        int m_ExecutedCount;
        public void Executed()
        {
            m_ExecutedCount++;
            if(m_ExecutedCount == Clips.Count)
            {
                m_ExecutedCount = 0;
                Delay?.Invoke();
                Delay = null;
            }
        }

        public override void Evaluate(float deltaTime) { }
        public override void Bind()
        {
            TrackPlayable = TimelineAnimationTrackPlayable.Create(this, Timeline.RootPlayable);
            PlayableIndex = Timeline.RootPlayable.GetInputCount() - 1;
            ClipPlayables = new List<TimelineAnimationClipPlayable>();

            if (m_PersistentMuted)
            {
                Timeline.RootPlayable.SetInputWeight(PlayableIndex, 0);
                return;
            }

            for (int i = 0; i < Clips.Count; i++)
            {
                ClipPlayables.Add(TimelineAnimationClipPlayable.Create(Clips[i] as AnimationClip, TrackPlayable.MixerPlayable, i));
            }

            if (AvatarMask)
                Timeline.RootPlayable.SetLayerMaskFromAvatarMask((uint)PlayableIndex, AvatarMask);
            else
                Timeline.RootPlayable.SetLayerMaskFromAvatarMask((uint)PlayableIndex, FullBodyMask);
        }
        public override void Unbind()
        {
            if (TrackPlayable != null)
            {
                Timeline.RootPlayable.DisconnectInput(PlayableIndex);
                TrackPlayable.Handle.Destroy();
                TrackPlayable = null;
            }
        }
        public override void SetTime(float time)
        {
            TrackPlayable.SetTime(time);
            ClipPlayables.ForEach(x => x.SetTime(time));
        }

        float m_OriginalWeight;
        public override void RuntimeMute(bool value)
        {
            if (PersistentMuted)
                return;

            if (value && !RuntimeMuted)
            {
                m_OriginalWeight = Timeline.RootPlayable.GetInputWeight(PlayableIndex);
                RuntimeMuted = true;
                Timeline.RootPlayable.SetInputWeight(PlayableIndex, value ? 0 : 1);
            }
            else if (!value && RuntimeMuted)
            {
                RuntimeMuted = false;
                Timeline.RootPlayable.SetInputWeight(PlayableIndex, m_OriginalWeight);
            }

        }

#if UNITY_EDITOR

        public override Type ClipType => typeof(AnimationClip);
        public override Clip AddClip(UnityEngine.Object referenceObject, int frame)
        {
            AnimationClip clip = new AnimationClip(referenceObject as UnityEngine.AnimationClip, this, frame);
            m_Clips.Add(clip);
            return clip;
        }
        public override bool DragValid()
        {
            return UnityEditor.DragAndDrop.objectReferences.Length == 1 && UnityEditor.DragAndDrop.objectReferences[0] as UnityEngine.AnimationClip;
        }


#endif
    }
    public class TimelineAnimationTrackPlayable : PlayableBehaviour
    {
        public AnimationTrack AnimationTrack { get; private set; }
        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        public AnimationMixerPlayable MixerPlayable { get; private set; }
        public Timeline Timeline => AnimationTrack.Timeline;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            AnimationTrack.Delay += () =>
            {
                if (AnimationTrack.RuntimeMuted)
                    return;

                float sumWeight = 0;
                foreach (var clipPlayable in AnimationTrack.ClipPlayables)
                {
                    sumWeight += clipPlayable.TargetWeight;
                }

                if (sumWeight == 0)
                {
                    Output.SetInputWeight(AnimationTrack.PlayableIndex, 0);
                }
                else if (0 < sumWeight && sumWeight < 1)
                {
                    Output.SetInputWeight(AnimationTrack.PlayableIndex, sumWeight);
                }
                else
                {
                    Output.SetInputWeight(AnimationTrack.PlayableIndex, 1);
                }
            };
        }

        public void SetTime(float time)
        {
            Handle.SetTime(time);
            MixerPlayable.SetTime(time);
            PrepareFrame(default, default);
        }

        public static TimelineAnimationTrackPlayable Create(AnimationTrack track, Playable output)
        {
            var handle = ScriptPlayable<TimelineAnimationTrackPlayable>.Create(track.Timeline.PlayableGraph);
            var timelineAnimationTrackPlayable = handle.GetBehaviour();
            timelineAnimationTrackPlayable.AnimationTrack = track;
            timelineAnimationTrackPlayable.Handle = handle;
            timelineAnimationTrackPlayable.MixerPlayable = AnimationMixerPlayable.Create(track.Timeline.PlayableGraph, track.Clips.Count);
            handle.AddInput(timelineAnimationTrackPlayable.MixerPlayable, 0, 1);

            timelineAnimationTrackPlayable.Output = output;
            output.AddInput(handle, 0, 0);

            return timelineAnimationTrackPlayable;
        }
    }


    [ScriptGuid("3f0d14cafa6f2c84389c42789ec00083"), Color(127, 253, 228)]
    public class AnimationClip : Clip
    {
        [ShowInInspector, OnValueChanged("OnClipChanged", "RebindTimeline")]
        public UnityEngine.AnimationClip Clip;
        [ShowInInspector, OnValueChanged("RebindTimeline")]
        public ExtraPolationMode ExtraPolationMode;


#if UNITY_EDITOR

        public override string Name => Clip ? Clip.name : base.Name;
        public override int Length => Clip ? Mathf.RoundToInt(Clip.length * TimelineUtility.FrameRate) : base.Length;
        public override ClipCapabilities Capabilities => ClipCapabilities.Resizable | ClipCapabilities.Mixable | ClipCapabilities.ClipInable;
        public AnimationClip(Track track, int frame) : base(track, frame) { }
        public AnimationClip(UnityEngine.AnimationClip clip, Track track, int frame) : base(track, frame)
        {
            Clip = clip;
            EndFrame = Length + frame;
        }
        void OnClipChanged()
        {
            OnNameChanged?.Invoke();
        }
#endif
    }
    public class TimelineAnimationClipPlayable : PlayableBehaviour
    {
        public AnimationClip AnimationClip { get; private set; }
        public AnimationTrack AnimationTrack => AnimationClip.Track as AnimationTrack;

        public int Index { get; private set; }
        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        public AnimationClipPlayable ClipPlayable { get; private set; }
        public float TargetWeight { get; private set; }

        float m_LastTime;
        float m_HandleTime;
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            m_HandleTime = (float)Handle.GetTime();
            float deltaTime = info.deltaTime;

            TimelineUtility.Lerp(m_HandleTime, deltaTime, Evaluate, ref m_LastTime);
            AnimationTrack.Executed();
        }

        public void SetTime(float time)
        {
            Handle.SetTime(time);
            TimelineUtility.Lerp(time, time, Evaluate, ref m_LastTime);
            AnimationTrack.Executed();
        }

        public void Evaluate(float deltaTime)
        {
            if (m_LastTime < AnimationClip.StartTime)
            {
                TargetWeight = 0;
                Output.SetInputWeight(Index, TargetWeight);
                ClipPlayable.SetTime(0);
            }
            else if (AnimationClip.StartTime <= m_LastTime && m_LastTime <= AnimationClip.EndTime)
            {
                float selfTime = m_LastTime - AnimationClip.StartTime;
                float remainTime = AnimationClip.EndTime - m_LastTime;
                ClipPlayable.SetTime(selfTime + AnimationClip.ClipInTime);

                if (selfTime < AnimationClip.EaseInTime)
                {
                    TargetWeight = selfTime / AnimationClip.EaseInTime;
                    if (AnimationClip.OtherEaseInTime > 0)
                    {
                        Output.SetInputWeight(Index, TargetWeight);
                    }
                    else
                    {
                        Output.SetInputWeight(Index, 1);
                    }
                }
                else if (remainTime < AnimationClip.EaseOutTime)
                {
                    TargetWeight = remainTime / AnimationClip.EaseOutTime;
                    if (AnimationClip.OtherEaseOutTime > 0)
                    {
                        Output.SetInputWeight(Index, TargetWeight);
                    }
                    else
                    {
                        Output.SetInputWeight(Index, 1);
                    }
                }
                else
                {
                    TargetWeight = 1;
                    Output.SetInputWeight(Index, TargetWeight);
                }
            }
            else if (m_LastTime > AnimationClip.EndTime)
            {
                ClipPlayable.SetTime(AnimationClip.DurationTime + AnimationClip.ClipInTime);
                switch (AnimationClip.ExtraPolationMode)
                {
                    case ExtraPolationMode.None:
                        TargetWeight = 0;
                        Output.SetInputWeight(Index, TargetWeight);
                        break;
                    case ExtraPolationMode.Hold:
                        //keep
                        break;
                }
            }
        }

        public static TimelineAnimationClipPlayable Create(AnimationClip clip, Playable output,int index)
        {
            var handle = ScriptPlayable<TimelineAnimationClipPlayable>.Create(clip.Timeline.PlayableGraph);
            var timelineAnimationClipPlayable = handle.GetBehaviour();
            timelineAnimationClipPlayable.AnimationClip = clip;
            timelineAnimationClipPlayable.Handle = handle;
            timelineAnimationClipPlayable.ClipPlayable = AnimationClipPlayable.Create(clip.Timeline.PlayableGraph, clip.Clip);
            timelineAnimationClipPlayable.ClipPlayable.SetApplyFootIK(false);
            handle.AddInput(timelineAnimationClipPlayable.ClipPlayable, 0, 1);

            timelineAnimationClipPlayable.Output = output;
            timelineAnimationClipPlayable.Index = index;
            output.ConnectInput(index, handle, 0, 0);

            return timelineAnimationClipPlayable;
        }
    }
}