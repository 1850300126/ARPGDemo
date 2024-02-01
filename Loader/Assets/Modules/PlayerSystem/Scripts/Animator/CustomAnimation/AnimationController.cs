using System.Collections;
using System.Collections.Generic;
using Taco.Timeline;
using UnityEngine;
using Custom.Animation;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System;
public class AnimationController : TimelinePlayer
{
        // [SerializeField] Animator animator;
        // private PlayableGraph graph;
        private AnimationMixerPlayable mixer;

        private AnimationNodeBase previousNode; // ��һ���ڵ�
        private AnimationNodeBase currentNode;  // ��ǰ�ڵ�
        private int inputPort0 = 0;
        private int inputPort1 = 1;


        private Coroutine transitionCoroutine;

        private float speed;
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                currentNode.SetSpeed(speed);
            }
        }

        public override void Init()
        {   
            // // ����ͼ
            // graph = PlayableGraph.Create("Animation_Controller");
            // // ����ͼ��ʱ��ģʽ
            // graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            // // ���������
            // mixer = AnimationMixerPlayable.Create(graph, 3);
            // // ����Output
            // AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);
            // // �û����������Output
            // playableOutput.SetSourcePlayable(mixer);

            base.Init();   
            mixer = AnimationMixerPlayable.Create(graph, 3);
            RootPlayable.AddInput(mixer, 0, 1);
        }

        public void DestoryNode(AnimationNodeBase node)
        {
            if (node != null)
            {
                graph.Disconnect(mixer, node.InputPort);
                node.PushPool();
            }
        }

        private void StartTransitionAniamtion(float fixedTime)
        {
            if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(TransitionAniamtion(fixedTime));
        }

        // ��������
        private IEnumerator TransitionAniamtion(float fixedTime)
        {
            // �����˿ں�
            int temp = inputPort0;
            inputPort0 = inputPort1;
            inputPort1 = temp;

            // Ӳ���ж�
            if (fixedTime == 0)
            {
                mixer.SetInputWeight(inputPort1, 0);
                mixer.SetInputWeight(inputPort0, 1);
            }

            // ��ǰ��Ȩ��
            float currentWeight = 1;
            float speed = 1 / fixedTime;

            while (currentWeight > 0)
            {
                // Ȩ���ڼ���
                currentWeight = Mathf.Clamp01(currentWeight - Time.deltaTime * speed);
                mixer.SetInputWeight(inputPort1, currentWeight);  // ����
                mixer.SetInputWeight(inputPort0, 1 - currentWeight); // ����
                yield return null;
            }
            transitionCoroutine = null;
        }

        /// <summary>
        /// ���ŵ�������
        /// </summary>
        public void PlaySingleAniamtion(UnityEngine.AnimationClip animationClip, float speed = 1, bool refreshAnimation = false, float transitionFixedTime = 0.25f)
        {
            SingleAnimationNode singleAnimationNode = null;
            if (currentNode == null) // �״β���
            {
                // singleAnimationNode = PoolSystem.instance.GetObject<SingleAnimationNode>();
                singleAnimationNode = new SingleAnimationNode();
                singleAnimationNode.Init(graph, mixer, animationClip, speed, inputPort0);
                mixer.SetInputWeight(inputPort0, 1);
            }
            else
            {
                SingleAnimationNode preNode = currentNode as SingleAnimationNode; // ��һ���ڵ�

                // ��ͬ�Ķ���
                if (!refreshAnimation && preNode != null && preNode.GetAnimationClip() == animationClip) return;
                // ���ٵ���ǰ���ܱ�ռ�õ�Node
                DestoryNode(previousNode);
                // singleAnimationNode = PoolSystem.instance.GetObject<SingleAnimationNode>();
                singleAnimationNode = new SingleAnimationNode();
                singleAnimationNode.Init(graph, mixer, animationClip, speed, inputPort1);
                previousNode = currentNode;
                StartTransitionAniamtion(transitionFixedTime);
            }
            this.speed = speed;
            currentNode = singleAnimationNode;
            // if (graph.IsPlaying() == false) graph.Play();
        }


        /// <summary>
        /// ���Ż�϶���
        /// </summary>
        public void PlayBlendAnimation(List<UnityEngine.AnimationClip> clips, float speed = 1, float transitionFixedTime = 0.25f)
        {
            // BlendAnimationNode blendAnimationNode = PoolSystem.instance.GetObject<BlendAnimationNode>();
            BlendAnimationNode blendAnimationNode = new BlendAnimationNode();
            // ����ǵ�һ�β��ţ������ڹ���
            if (currentNode == null)
            {
                blendAnimationNode.Init(graph, mixer, clips, speed, inputPort0);
                mixer.SetInputWeight(inputPort0, 1);
            }
            else
            {
                DestoryNode(previousNode);
                blendAnimationNode.Init(graph, mixer, clips, speed, inputPort1);
                previousNode = currentNode;
                StartTransitionAniamtion(transitionFixedTime);
            }
            this.speed = speed;
            currentNode = blendAnimationNode;
            if (graph.IsPlaying() == false) graph.Play();
        }

        /// <summary>
        /// ���Ż�϶���
        /// </summary>
        public void PlayBlendAnimation(UnityEngine.AnimationClip clip1, UnityEngine.AnimationClip clip2, float speed = 1, float transitionFixedTime = 0.25f)
        {
            BlendAnimationNode blendAnimationNode = new BlendAnimationNode();
            // ����ǵ�һ�β��ţ������ڹ���
            if (currentNode == null)
            {
                blendAnimationNode.Init(graph, mixer, clip1, clip2, speed, inputPort0);
                mixer.SetInputWeight(inputPort0, 1);
            }
            else
            {
                DestoryNode(previousNode);
                blendAnimationNode.Init(graph, mixer, clip1, clip2, speed, inputPort1);
                previousNode = currentNode;
                StartTransitionAniamtion(transitionFixedTime);
            }
            this.speed = speed;
            currentNode = blendAnimationNode;
            if (graph.IsPlaying() == false) graph.Play();
        }

        public void SetBlendWeight(List<float> weightList)
        {
            (currentNode as BlendAnimationNode).SetBlendWeight(weightList);
        }
        public void SetBlendWeight(float clip1Weight)
        {
            (currentNode as BlendAnimationNode).SetBlendWeight(clip1Weight);
        }


        private void OnDestroy()
        {
            graph.Destroy();
        }

        protected override void OnDisable()
        {   
            base.OnDisable();
            graph.Stop();
        }

        #region RootMotion
        private Action<Vector3, Quaternion> rootMotionAction;
        private void OnAnimatorMove()
        {   
            rootMotionAction?.Invoke(Animator.deltaPosition, Animator.deltaRotation);
        }
        public void SetRootMotionAction(Action<Vector3, Quaternion> rootMotionAction)
        {
            this.rootMotionAction = rootMotionAction;
        }
        public void ClearRootMotionAction()
        {
            rootMotionAction = null;
        }
        #endregion
        #region �����¼�
        private Dictionary<string, Action> eventDic = new Dictionary<string, Action>();
        // Animator�ᴥ����ʵ���¼�����
        private void AniamtionEvent(string eventName)
        {
            if (eventDic.TryGetValue(eventName, out Action action))
            {
                action?.Invoke();
            }
        }
        public void AddAniamtionEvent(string eventName, Action action)
        {
            if (eventDic.TryGetValue(eventName, out Action _action))
            {
                _action += action;
            }
            else
            {
                eventDic.Add(eventName, action);
            }
        }

        public void RemoveAnimationEvent(string eventName)
        {
            eventDic.Remove(eventName);
        }

        public void RemoveAnimationEvent(string eventName, Action action)
        {
            if (eventDic.TryGetValue(eventName, out Action _action))
            {
                _action -= action;
            }
        }

        public void CleanAllActionEvent()
        {
            eventDic.Clear();
        }

        public void PlayTimeline(Timeline timeline, Action value)
        {
            Timeline _timeline = Instantiate(timeline);
            AddTimeline(_timeline);

            timeline.OnDone += () => 
            {
                value?.Invoke();
                Debug.Log(1);
                RemoveTimeline(timeline);
            };
        }
    #endregion
}
