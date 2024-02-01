using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Taco.Editor 
{
    public enum DragLineDirection { Top, Down, Left, Right }

    public class DragLineManipulator : PointerManipulator
    {
        DragLineDirection m_Direction;

        Action<PointerDownEvent> m_OnDragStart;
        Action m_OnDragStop;
        Action<Vector2> m_OnDragMove;
        
        public bool Active { get; private set; }
        public IMGUIContainer Handle { get; private set; }
        
        Vector3 m_Start;
        
        public float Size = 4;
        public float Offset = 0;
        public bool Enable = true;

        public DragLineManipulator(DragLineDirection dragLineDirection, Action<Vector2> onDragMove)
        {
            m_OnDragMove = onDragMove;
            Active = false;
            m_Direction = dragLineDirection;
            activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
        }
        public DragLineManipulator(DragLineDirection dragLineDirection, Action<Vector2> onDragMove, Action<PointerDownEvent> onDragStart, Action onDragStop) : this(dragLineDirection, onDragMove)
        {
            m_OnDragStart = onDragStart;
            m_OnDragStop = onDragStop;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            Handle = new IMGUIContainer(() =>
            {
                if(!Enable || Active)
                    return;

                switch (m_Direction)
                {
                    case DragLineDirection.Top:
                        EditorGUIUtility.AddCursorRect(new Rect(0, 0, target.worldBound.width, Size), MouseCursor.ResizeVertical);
                        break;
                    case DragLineDirection.Down:
                        EditorGUIUtility.AddCursorRect(new Rect(0, 0, target.worldBound.width, Size), MouseCursor.ResizeVertical);
                        break;
                    case DragLineDirection.Left:
                        EditorGUIUtility.AddCursorRect(new Rect(0, 0, Size, target.worldBound.height), MouseCursor.ResizeHorizontal);
                        break;
                    case DragLineDirection.Right:
                        EditorGUIUtility.AddCursorRect(new Rect(0, 0, Size, target.worldBound.height), MouseCursor.ResizeHorizontal);
                        break;
                }
            });
            Handle.style.position = Position.Absolute;
            Handle.style.marginTop = Handle.style.marginBottom = Handle.style.marginLeft = Handle.style.marginRight = 0;
            switch (m_Direction)
            {
                case DragLineDirection.Top:
                    Handle.style.top = -target.style.borderTopWidth.value + Offset;
                    Handle.style.width = Length.Percent(100);
                    Handle.style.height = Size;
                    break;
                case DragLineDirection.Down:
                    Handle.style.bottom = -target.style.borderBottomWidth.value + Offset;
                    Handle.style.width = Length.Percent(100);
                    Handle.style.height = Size;
                    break;
                case DragLineDirection.Left:
                    Handle.style.left = -target.style.borderLeftWidth.value + Offset;
                    Handle.style.width = Size;
                    Handle.style.height = Length.Percent(100);
                    break;
                case DragLineDirection.Right:
                    Handle.style.right = -target.style.borderRightWidth.value + Offset;
                    Handle.style.width = Size;
                    Handle.style.height = Length.Percent(100);
                    break;
            }
            Handle.RegisterCallback<PointerDownEvent>(OnPointerDown);
            Handle.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            Handle.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            target.Add(Handle);
        }
        protected override void UnregisterCallbacksFromTarget()
        {
            Handle.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            Handle.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            Handle.UnregisterCallback<PointerUpEvent>(OnPointerUp);

            target.Remove(Handle);
            Handle = null;
        }

        protected void OnPointerDown(PointerDownEvent e)
        {
            if (!Enable) return;

            if (Active)
            {
                e.StopImmediatePropagation();
            }
            else if (CanStartManipulation(e))
            {
                m_Start = e.localPosition;
                Active = true;
                Handle.CapturePointer(e.pointerId);
                e.StopPropagation();

                m_OnDragStart?.Invoke(e);
            }
        }
        protected void OnPointerMove(PointerMoveEvent e)
        {
            if (!Enable) return;

            if (Active && Handle.HasPointerCapture(e.pointerId))
            {
                Vector2 delta = e.localPosition - m_Start;
                ApplyDelta(delta);
                e.StopPropagation();
            }
        }
        protected void OnPointerUp(PointerUpEvent e)
        {
            if (!Enable) return;

            if (Active && CanStopManipulation(e))
            {
                Active = false;
                Handle.ReleasePointer(e.pointerId);
                e.StopPropagation();

                m_OnDragStop?.Invoke();
            }
        }
        protected void ApplyDelta(Vector2 delta)
        {
            m_OnDragMove?.Invoke(delta);
        }
    }
}