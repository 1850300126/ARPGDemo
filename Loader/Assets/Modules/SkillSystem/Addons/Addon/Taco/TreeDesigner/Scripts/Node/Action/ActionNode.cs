using System;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeColor(239, 71, 111)]
    [Input("Input")]
    public abstract partial class ActionNode : RunnableNode
    {
        [SerializeField]
        string m_InputEdgeGUID;
        public string InputEdgeGUID => m_InputEdgeGUID;

        [NonSerialized]
        protected RunnableNode m_Parent;
        public RunnableNode Parent => m_Parent;

        public virtual State ReturnState => State.Success;

        public override void Init(BaseTree tree)
        {
            base.Init(tree);

            if (!string.IsNullOrEmpty(m_InputEdgeGUID) && m_Owner.GUIDEdgeMap.ContainsKey(m_InputEdgeGUID))
                m_Parent = m_Owner.GUIDEdgeMap[m_InputEdgeGUID].StartNode as RunnableNode;
        }
        public override void Dispose()
        {
            base.Dispose();

            m_Parent = null;
        }
        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            m_InputEdgeGUID = string.Empty;
            m_Parent = null;
        }

        protected override void OnStart()
        {
            base.OnStart();
            DoAction();
        }
        protected override State OnUpdate()
        {
            return ReturnState;
        }

        protected abstract void DoAction();


#if UNITY_EDITOR
        public override void OnInputLinked(BaseEdge edge)
        {
            base.OnInputLinked(edge);
            m_InputEdgeGUID = edge.GUID;
            m_Parent = edge.StartNode as RunnableNode;
        }
        public override void OnInputUnlinked(BaseEdge edge)
        {
            base.OnInputUnlinked(edge);

            m_InputEdgeGUID = string.Empty;
            m_Parent = null;
        }

        public override void OnMoved()
        {
            base.OnMoved();
            if (m_Parent is CompositeNode compositeNode)
                compositeNode.OrderChildren();
        }
#endif
    }
}