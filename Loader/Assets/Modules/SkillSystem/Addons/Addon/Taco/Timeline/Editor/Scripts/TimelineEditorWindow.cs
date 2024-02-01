using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Taco.Editor;

namespace Taco.Timeline.Editor
{
    public class TimelineEditorWindow : EditorWindow, ISelection
    {
        VisualElement m_Top;
        VisualElement m_LeftPanel;
        VisualElement m_TrackHierarchy;
        VisualElement m_Toolbar;
        VisualElement m_TrackHandleContainer;
        VisualElement m_AddTrackButton;

        Button m_PlayButton;
        Button m_PauseButton;
        ObjectField m_TargetField;

        TimelineFieldView m_TimelineField;

        public Timeline Timeline { get; private set; }
        public SerializedObject SerializedTimeline { get; private set; }

        public virtual void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = Resources.Load<VisualTreeAsset>("VisualTree/TimelineEditorWindow");
            visualTree.CloneTree(root);
            root.AddToClassList("timelineEditorWindow");

            m_Top = root.Q("top");
            m_Top.SetEnabled(false);

            m_PlayButton = root.Q<Button>("play-button");
            m_PlayButton.clicked += () =>
            {
                Timeline.TimelinePlayer.IsPlaying = true;
            };

            m_PauseButton = root.Q<Button>("pause-button");
            m_PauseButton.clicked += () =>
            {
                Timeline.TimelinePlayer.IsPlaying = false;
            };

            m_TargetField = root.Q<ObjectField>("target-field");
            m_TargetField.objectType = typeof(TimelinePlayer);
            m_TargetField.allowSceneObjects = true;
            m_TargetField.RegisterValueChangedCallback((e) =>
            {
                if (!EditorUtility.IsPersistent(e.newValue) && e.newValue is TimelinePlayer timelinePlayer && Timeline.TimelinePlayer != timelinePlayer)
                {
                    if (Timeline.TimelinePlayer)
                    {
                        Timeline.TimelinePlayer.Dispose();
                    }

                    if (!timelinePlayer.IsValid)
                    {
                        timelinePlayer.Init();
                        timelinePlayer.AddTimeline(Timeline);
                    }
                }
                else if(e.newValue == null)
                {
                    if (Timeline.TimelinePlayer)
                    {
                        Timeline.TimelinePlayer.Dispose();
                    }
                }
                else
                {
                    m_TargetField.SetValueWithoutNotify(null);
                }
            });
            m_TargetField.SetEnabled(!Application.isPlaying);

            m_LeftPanel = root.Q("left-panel");
            m_LeftPanel.SetEnabled(false);

            m_TrackHierarchy = root.Q("track-hierarchy");
            m_Toolbar = root.Q("tool-bar");
            m_TrackHandleContainer = root.Q("track-handle-container");
            m_TrackHandleContainer.focusable = true;
            m_TrackHandleContainer.RegisterCallback<KeyDownEvent>((e) =>
            {
                switch (e.keyCode)
                {
                    case KeyCode.Delete:
                        {
                            ApplyModify(() =>
                            {
                                var selectableToRemove = Selections.ToList();
                                foreach (var selectable in selectableToRemove)
                                {
                                    if (selectable is TimelineTrackHandle trackHandle)
                                    {
                                        Timeline.RemoveTrack(trackHandle.Track);
                                    }
                                }
                            }, "Remove");
                        }
                        break;
                }
            });
            m_TrackHandleContainer.RegisterCallback<PointerDownEvent>((e) =>
            {
                foreach (var timelineTrackHandle in m_TrackHandleContainer.Query<TimelineTrackHandle>().ToList())
                {
                    if (timelineTrackHandle.worldBound.Contains(e.position))
                    {
                        timelineTrackHandle.OnPointerDown(e);
                        e.StopImmediatePropagation();
                        return;
                    }
                }

                if (e.button == 0)
                {
                    m_TimelineField.ClearSelection();
                    e.StopImmediatePropagation();
                }
            });


            m_AddTrackButton = root.Q("add-track-button");
            m_AddTrackButton.AddManipulator(new DropdownMenuManipulator((menu) =>
            {
                List<(Type, float)> types = new List<(Type, float)>();
                foreach (var trackScriptPair in TimelineEditorUtility.TrackScriptMap)
                {
                    float index = trackScriptPair.Key.GetAttribute<OrderedAttribute>()?.Index ?? 0;
                    types.Add((trackScriptPair.Key, index));
                }
                types = types.OrderBy(i => i.Item2).ToList();
                foreach (var type in types.OrderBy(i=>i.Item2))
                {

                    menu.AppendAction(type.Item1.Name, (e) =>
                    {
                        AddTrack(type.Item1);
                    });
                }

            }, MouseButton.LeftMouse));

            m_TimelineField = root.Q<TimelineFieldView>();
            m_TimelineField.SetEnabled(false);
            m_TimelineField.EditorWindow = this;
            m_TimelineField.OnPopulatedCallback += PopulateView;
            m_TimelineField.OnGeometryChangedCallback += () =>
            {
                m_Toolbar.style.top = m_TimelineField.MarkerField.worldBound.yMin - 47;
            };

            EditorApplication.playModeStateChanged += (e) =>
            {
                Dispose();
            };
            Undo.undoRedoEvent += OnUndoRedoEvent;
            UpdateBindState();
        }

        private void OnDestroy()
        {
            Dispose();
            Undo.undoRedoEvent += OnUndoRedoEvent;
        }

        private void Update()
        {
            if(EditorApplication.isCompiling)
                Close();
        }

        private void OnFocus()
        {

        }
        private void OnLostFocus()
        {

        }
        private void OnSelectionChange()
        {
            if (Selection.activeObject is Timeline timeline && timeline != Timeline)
            {
                Dispose();
                Init(timeline);
            }
            
        }

        public void Init(Timeline timeline)
        {
            if (Timeline == timeline) return;

            timeline.Init();
            Timeline = timeline;
            SerializedTimeline = new SerializedObject(timeline);

            Timeline.OnValueChanged += m_TimelineField.PopulateView;
            Timeline.OnEvaluated += m_TimelineField.UpdateTimeLocator;
            Timeline.OnBindStateChanged += m_TimelineField.UpdateBindState;
            Timeline.OnBindStateChanged += UpdateBindState;

            m_Top.SetEnabled(true);
            m_LeftPanel.SetEnabled(true);
            m_TimelineField.SetEnabled(true);
            UpdateBindState();

            EditorCoroutineHelper.WaitWhile(m_TimelineField.PopulateView, () => m_TimelineField.ContentWidth == 0);
        }
        public void Dispose()
        {
            if (Timeline)
            {
                if(Timeline.TimelinePlayer && Timeline.TimelinePlayer.RunningTimelines.Count == 1 && !Application.isPlaying)
                    Timeline.TimelinePlayer.Dispose();

                Timeline.OnValueChanged -= m_TimelineField.PopulateView;
                Timeline.OnEvaluated -= m_TimelineField.UpdateTimeLocator;
                Timeline.OnBindStateChanged -= m_TimelineField.UpdateBindState;
                Timeline.OnBindStateChanged -= UpdateBindState;
                Timeline = null;
                SerializedTimeline = null;
            }
            m_Top.SetEnabled(false);
            m_LeftPanel.SetEnabled(false);
            m_TimelineField.SetEnabled(false);
            m_TimelineField.PopulateView();
            UpdateBindState();
        }
        public void PopulateView()
        {
            m_TrackHandleContainer.Clear();
            m_Elements.Clear();
            m_Selections.Clear();

            if (Timeline != null)
            {
                foreach (var trackView in m_TimelineField.TrackViews)
                {
                    TimelineTrackHandle trackHandle = new TimelineTrackHandle(trackView);
                    trackHandle.SelectionContainer = this;
                    m_TrackHandleContainer.Add(trackHandle);
                    m_Elements.Add(trackHandle);
                }
            }
        }

        public void ApplyModify(Action action, string name)
        {
            Undo.RegisterCompleteObjectUndo(Timeline, $"Timeline: {name}");
            SerializedTimeline.Update();
            action?.Invoke();
            EditorUtility.SetDirty(Timeline);
        }
        public void UpdateSerializedTimeline()
        {
            SerializedTimeline = new SerializedObject(Timeline);
        }
        public void AddTrack(Type type)
        {
            ApplyModify(() =>
            {
                Timeline.AddTrack(type);
            }, "Add Track");
        }

        void OnUndoRedoEvent(in UndoRedoInfo info)
        {
            if (info.undoName.Split(':')[0] == "Timeline")
            {
                Timeline?.Init();
            }
        }

        void UpdateBindState()
        {
            if (Timeline && Timeline.TimelinePlayer)
            {
                m_PlayButton.SetEnabled(true);
                m_PauseButton.SetEnabled(true);
                m_TargetField.SetValueWithoutNotify(Timeline.TimelinePlayer);
            }
            else
            {
                m_PlayButton.SetEnabled(false);
                m_PauseButton.SetEnabled(false);
                m_TargetField.SetValueWithoutNotify(null);
            }
        }

        #region Selection
        public VisualElement ContentContainer => m_TrackHandleContainer;

        protected List<ISelectable> m_Elements = new List<ISelectable>();
        public List<ISelectable> Elements => m_Elements;

        protected List<ISelectable> m_Selections = new List<ISelectable>();
        public List<ISelectable> Selections => m_Selections;
        public void AddToSelection(ISelectable selectable)
        {
            m_Selections.Add(selectable);
            selectable.Select();
        }
        public void RemoveFromSelection(ISelectable selectable)
        {
            m_Selections.ForEach(i => i.Unselect());
            Selections.Clear();
        }
        public void ClearSelection()
        {
            m_Selections.ForEach(i => i.Unselect());
            Selections.Clear();
        }
        #endregion

        [MenuItem("Tools/TimelineEditor", false, 0)]
        public static void OpenTimelineEditorWindow()
        {
            GetWindow<TimelineEditorWindow>();
        }

        [UnityEditor.Callbacks.OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);
            var timeline = obj as Timeline;
            if (timeline == null) return false;
            GetWindow<TimelineEditorWindow>().Init(timeline);
            return true;
        }
    }
}