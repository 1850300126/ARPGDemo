using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Taco.Timeline
{

    [RequireComponent(typeof(Animator))]
    public class TimelinePlayer : MonoBehaviour
    {
        public RuntimeAnimatorController Controller;
        public bool ApplyRootMotion;

        bool m_IsPlaying;
        public bool IsPlaying
        {
            get => m_IsPlaying;
            set
            {
                if (m_IsPlaying == value)
                    return;

                m_IsPlaying = value;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (m_IsPlaying)
                    {
                        UnityEditor.EditorApplication.update += EditorUpdate;
                    }
                    else
                    {
                        UnityEditor.EditorApplication.update -= EditorUpdate;
                    }
                }
#endif
            }
        }
        public bool IsValid => graph.IsValid();
        public Animator Animator { get; private set; }
        public PlayableGraph graph { get; private set; }
        public AnimationLayerMixerPlayable RootPlayable { get; private set; }
        public AnimatorControllerPlayable CtrlPlayable { get; private set; }
        public List<Timeline> RunningTimelines { get; private set; }
        public float AdditionalDelta { get; set; }

        public event Action OnEvaluated;

        protected virtual void OnEnable()
        {
            Init();
            IsPlaying = true;
        }
        protected virtual void OnDisable()
        {
            Dispose();
        }
        protected virtual void Update()
        {
            if (IsPlaying)
            {
                Evaluate(Time.deltaTime);
                if (AdditionalDelta > 0)
                {
                    Evaluate(AdditionalDelta);
                    AdditionalDelta = 0;
                }
            }
        }
        // private void OnAnimatorMove() { }


        public virtual void Init()
        {
            Animator = GetComponent<Animator>();
            graph = PlayableGraph.Create("Taco.Timeline.PlayableGraph");

            RootPlayable = AnimationLayerMixerPlayable.Create(graph);

            AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(graph, "Animation", Animator);
            playableOutput.SetSourcePlayable(RootPlayable);

            // CtrlPlayable = AnimatorControllerPlayable.Create(graph, Controller);
            // RootPlayable.AddInput(CtrlPlayable, 0, 1);

            RunningTimelines = new List<Timeline>();
        }
        public virtual void Dispose()
        {
            if (IsValid)
            {
                for (int i = RunningTimelines.Count - 1; i >= 0; i--)
                {
                    RemoveTimeline(RunningTimelines[i]);
                }
                graph.Destroy();
            }
            RunningTimelines = null;
            IsPlaying = false;
        }
        public virtual void Evaluate(float deltaTime)
        {
            for (int i = RunningTimelines.Count - 1; i >= 0; i--)
            {
                Timeline runningTimelines = RunningTimelines[i];
                runningTimelines.Evaluate(deltaTime);
            }
            graph.Evaluate(deltaTime);

            OnRootMotion();

            OnEvaluated?.Invoke();
        }
        protected virtual void OnRootMotion()
        {
            if (ApplyRootMotion)
                transform.position += Animator.deltaPosition;
        }

        #region Aniamtor
        public virtual void SetFloat(string name, float value)
        {
            CtrlPlayable.SetFloat(name, value);
        }
        public virtual float GetFloat(string name)
        {
            return CtrlPlayable.GetFloat(name);
        }

        public virtual void SetBool(string name, bool value)
        {
            CtrlPlayable.SetBool(name, value);
        }
        public virtual bool GetBool(string name)
        {
            return CtrlPlayable.GetBool(name);
        }

        public virtual void SetTrigger(string name)
        {
            CtrlPlayable.SetTrigger(name);
        }

        public virtual void SetStateTime(string name, float time, int layer)
        {
            CtrlPlayable.CrossFadeInFixedTime(name, 0, layer, time);
        }
        #endregion

        public virtual void AddTimeline(Timeline timeline)
        {
            timeline.Init();
            timeline.Bind(this);
            RunningTimelines.Add(timeline);
        }
        public virtual void RemoveTimeline(Timeline timeline)
        {
            timeline.Unbind();
            RunningTimelines.Remove(timeline);
            if (RunningTimelines.Count == 0)
                RootPlayable.SetInputCount(1);
        }

#if UNITY_EDITOR

        public void EditorUpdate()
        {
            Evaluate((float)Editor.TacoEditorUtility.DeltaTime);
            if (AdditionalDelta > 0)
            {
                Evaluate(AdditionalDelta);
                AdditionalDelta = 0;
            }
        }
#endif
    }
}