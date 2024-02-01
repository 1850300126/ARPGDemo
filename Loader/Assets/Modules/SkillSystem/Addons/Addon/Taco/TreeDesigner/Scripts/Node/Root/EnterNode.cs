using System;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("GetNodeName")]
    [NodeColor(217, 187, 249)]
    [Output("Output", PortCapacity.Single)]
    public class EnterNode : RunnableNode
    {
        [SerializeField]
        protected string m_OutputEdgeGUID;
        public string OutputGUID => m_OutputEdgeGUID;

        [NonSerialized]
        protected RunnableNode m_Child;
        public RunnableNode Child => m_Child;

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
        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            m_OutputEdgeGUID = string.Empty;
            m_Child = null;
        }
        public override void ResetNode()
        {
            base.ResetNode();
            m_Child?.ResetNode();
        }

        protected override State OnUpdate()
        {
            if (m_Child)
                return m_Child.UpdateNode();
            else
                return State.None;
        }

#if UNITY_EDITOR

        public override NodeCapabilities Capabilities => base.Capabilities & ~NodeCapabilities.Deletable & ~NodeCapabilities.Copiable & ~NodeCapabilities.Groupable & ~NodeCapabilities.Stackable;
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

        public string NodeName;

        string GetNodeName()
        {
            return NodeName;
        }
    }
#endif
}