using System;
using System.Collections.Generic;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeColor(217, 187, 249)]
    [Output("Output", PortCapacity.Single)]
    public abstract partial class TriggerNode : RunnableNode
    {
        [SerializeField]
        protected string m_OutputEdgeGUID;
        public string OutputGUID => m_OutputEdgeGUID;

        [NonSerialized]
        protected RunnableNode m_Child;
        public RunnableNode Child => m_Child;

        Queue<Action> m_Actions = new Queue<Action>();

        public override void Init(BaseTree tree)
        {
            base.Init(tree);
            if (!string.IsNullOrEmpty(m_OutputEdgeGUID) && m_Owner.GUIDEdgeMap.ContainsKey(m_OutputEdgeGUID))
                m_Child = m_Owner.GUIDEdgeMap[m_OutputEdgeGUID].EndNode as RunnableNode;
        }
        public override void Dispose()
        {
            base.Dispose();
            m_Child = null;
        }
        public override void ResetNode()
        {
            base.ResetNode();
            m_Child?.ResetNode();
        }

        protected override State OnUpdate()
        {
            m_Child?.ResetNode();
            return m_Child?.UpdateNode() ?? State.Success;
        }
        protected override void OnStop()
        {
            base.OnStop();
            if (m_Actions.Count > 0)
                m_Actions.Dequeue()?.Invoke();
        }
        protected override void OnReset()
        {
            base.OnReset();
            m_Actions.Clear();
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            m_OutputEdgeGUID = string.Empty;
            m_Child = null;
        }

        public abstract void Register();
        public abstract void Unregister();
        public virtual void OnTriggered()
        {
            if (State == State.Running)
                m_Actions.Enqueue(() => UpdateNode());
            else
                UpdateNode();
        }
    }

#if UNITY_EDITOR
    public abstract partial class TriggerNode : RunnableNode
    {
        public override void OnOutputLinked(BaseEdge edge)
        {
            base.OnOutputLinked(edge);

            m_OutputEdgeGUID = edge.GUID;
            m_Child = edge.EndNode as RunnableNode;
        }
        public override void OnOutputUnlinked(BaseEdge edge)
        {
            base.OnOutputUnlinked(edge);

            m_OutputEdgeGUID = string.Empty;
            m_Child = null;
        }
    }
#endif
}