using System;
using UnityEngine;

namespace Taco.Timeline
{
    [ScriptGuid("31085f11443fe1347b871c5d69db3774"), IconGuid("e28acf5dc5b2e3d4a97920bf4e831c87"), Ordered(3), Color(201, 060, 032)]
    public class TreeTrack : Track
    {
#if UNITY_EDITOR

        public override Type ClipType => typeof(TreeClip);

        public override Clip AddClip(UnityEngine.Object referenceObject, int frame)
        {
            TreeClip clip = new TreeClip(referenceObject as TimelineRunningTree, this, frame);
            m_Clips.Add(clip);
            return clip;
        }
        public override bool DragValid()
        {
            return UnityEditor.DragAndDrop.objectReferences.Length == 1 && UnityEditor.DragAndDrop.objectReferences[0] is TimelineRunningTree;
        }

#endif
    }

    [ScriptGuid("31085f11443fe1347b871c5d69db3774"), Color(201, 060, 032)]
    public partial class TreeClip : Clip
    {
        [ShowInInspector, OnValueChanged("OnClipChanged", "RebindTimeline"), HorizontalGroup("TreePrefab")]
        public TimelineRunningTree TreePrefab;
        [ShowInInspector, ReadOnly, HorizontalGroup("TreeInstance"), ShowIf("ShowIf")]
        public TimelineRunningTree TreeInstance;

        public override void Bind()
        {
            base.Bind();

            Instantiate();

#if UNITY_EDITOR
            if (TreeInstance)
                TreeInstance.OnModified += OnTreeModified;
#endif

        }
        public override void Unbind()
        {
            base.Unbind();

#if UNITY_EDITOR
            if (TreeInstance)
                TreeInstance.OnModified -= OnTreeModified;
#endif

            Destroy();
        }
        public override void Evaluate(float deltaTime)
        {
            base.Evaluate(deltaTime);
            if (TreeInstance && Active)
            {
                TreeInstance.UpdateTree(deltaTime);
            }
        }
        public override void OnEnable()
        {
            TreeInstance.OnTreeEnable();
        }
        public override void OnDisable()
        {
            TreeInstance.OnTreeDisable();
        }


        void Instantiate()
        {
            if (TreePrefab)
            {
                if (Application.isPlaying)
                {
                    TreeInstance = UnityEngine.Object.Instantiate(TreePrefab);
                    TreeInstance.InitTree(this);
                }
                else
                {
                    TreeInstance = TreePrefab;
                    TreeInstance.InitTree(this);
                }
            }
        }
        void Destroy()
        {
            if (Application.isPlaying)
            {
                if (TreeInstance)
                {
                    TreeInstance.OnTreeDestroy();
                    TreeInstance.DisposeTree();
                    UnityEngine.Object.Destroy(TreeInstance);
                    TreeInstance = null;
                }
            }
            else
            {
                if (TreeInstance)
                {
                    TreeInstance.OnTreeDestroy();
                    TreeInstance.DisposeTree();
                    TreeInstance = null;
                }
            }
        }

#if UNITY_EDITOR

        public override string Name => TreePrefab ? TreePrefab.name : base.Name;
        public override ClipCapabilities Capabilities => ClipCapabilities.Resizable;

        public TreeClip(Track track, int frame) : base(track, frame)
        {
        }
        public TreeClip(TimelineRunningTree tree, Track track, int frame) : base(track, frame)
        {
            TreePrefab = tree;
        }

        void OnTreeModified()
        {
            Timeline.RebindTrack(Track);
        }
        void OnClipChanged()
        {
            OnNameChanged?.Invoke();
        }
        [Button("Open"), HorizontalGroup("TreePrefab")]
        void OpenTreePrefab()
        {
            if (TreePrefab)
            {
                TreeDesigner.Editor.TreeWindowUtility.OpenTree(TreePrefab);
            }
        }
        [Button("Open"), HorizontalGroup("TreeInstance"), ShowIf("ShowIf")]
        void OpenTreeInstance()
        {
            if (TreeInstance)
            {
                TreeDesigner.Editor.TreeWindowUtility.OpenTree(TreeInstance);
            }
        }
        bool ShowIf()
        {
            return Application.isPlaying;
        }
#endif
    }
}