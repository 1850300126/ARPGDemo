using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillMultiLineTrackStyle : SkillTrackStyleBase
{
    #region 常量
    private const string menuAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenu.uxml";
    private const string trackAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackContent.uxml";
    private const float headHeight = 35;//5是间距
    private const float itemHeight = 32;//2是底部外边距

    #endregion


    private Action addChildTrackAction;
    private Func<int, bool> deleteChildTrackFunc;
    private Action<int, int> swapChildAction;
    private Action<ChildTrack, string> updateTrackNameAction;
    private VisualElement menuItemParent;//子轨道的菜单父物体
    public List<ChildTrack> childTracksList = new List<ChildTrack>();


    public void Init(VisualElement menuParent, VisualElement contentParent, string title, Action addChildTrackAction, Func<int, bool> deleteChildTrackFunc, Action<int, int> swapChildAction, Action<ChildTrack, string> updateTrackNameAction)
    {
        this.menuParent = menuParent;
        this.contentParent = contentParent;
        this.addChildTrackAction = addChildTrackAction;
        this.deleteChildTrackFunc = deleteChildTrackFunc;
        this.swapChildAction = swapChildAction; 
        this.updateTrackNameAction = updateTrackNameAction;

        menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(menuAssetPath).Instantiate().Query().ToList()[1];//不要容器，直接持有目标物体
        contentRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackAssetPath).Instantiate().Query().ToList()[1];//不要容器，直接持有目标物体
        menuParent.Add(menuRoot);
        contentParent.Add(contentRoot);

        titleLabel = menuRoot.Q<Label>("Title");
        titleLabel.text = title;

        menuItemParent = menuRoot.Q<VisualElement>("TrackMenuList");

        menuItemParent.RegisterCallback<MouseDownEvent>(ItemParentMouseDown);
        menuItemParent.RegisterCallback<MouseMoveEvent>(ItemParentMouseMove);
        menuItemParent.RegisterCallback<MouseUpEvent>(ItemParentMouseUp);
        menuItemParent.RegisterCallback<MouseOutEvent>(ItemParentMouseOut);

        //添加子轨道的按钮
        Button addButton = menuRoot.Q<Button>("AddButton");
        addButton.clicked += AddButtonClicked;

        UpdateSize();
    }

    #region 子轨道鼠标交互

    private bool isDragging = false;
    private int selectTrackIndex = -1;

    private void ItemParentMouseDown(MouseDownEvent evt)
    {   
        // 关闭旧的
        if(selectTrackIndex != -1)
        {
            childTracksList[selectTrackIndex].UnSelect();
        }


        // 通过本地高度推导出当前交互的是第几个轨道
        // 加工获取值，避免四舍五入而导致无法选中正确的轨道
        float mousePosition = evt.localMousePosition.y - itemHeight / 2;
        selectTrackIndex = GetChildIndexByMousePosition(mousePosition);
        childTracksList[selectTrackIndex].Select();
        // TODO:拖拽
        isDragging = true;
    }
    private void ItemParentMouseMove(MouseMoveEvent evt)
    {
        if(selectTrackIndex == -1 || isDragging == false) return;   

        float mousePosition = evt.localMousePosition.y - itemHeight / 2;
        int mouseTrackIndex = GetChildIndexByMousePosition(mousePosition);

        // 不和自己交换
        if(mouseTrackIndex != selectTrackIndex)
        {   
            SwpChildTrack(selectTrackIndex, mouseTrackIndex);
            // 把选中的轨道更新为拖拽的轨道
            selectTrackIndex = mouseTrackIndex;
        }
    }
    private void ItemParentMouseUp(MouseUpEvent evt)
    {
        isDragging = false;
    }
    private void ItemParentMouseOut(MouseOutEvent evt)
    {
        // ItemParentMouseOut会无意义调用，因为子物体和我们产生遮挡
        // 检测鼠标是否真的离开了我们的检测范围
        if(!menuItemParent.contentRect.Contains(evt.localMousePosition))
        {
            isDragging = false;
        }

    }

    private int GetChildIndexByMousePosition(float mousePositionY)
    {   
        // 算出鼠标选择的是哪个轨道
        int trackIndex = Mathf.RoundToInt(mousePositionY / itemHeight);
        // 安全校验, 避免索引超过count
        trackIndex = Mathf.Clamp(trackIndex, 0, childTracksList.Count - 1);
        return trackIndex;
        
    }
    #endregion

    // 交换子轨道
    private void SwpChildTrack(int index1, int index2)
    {
        if(index1 == index2) return;

        ChildTrack childTrack1 = childTracksList[index1];
        ChildTrack childTrack2 = childTracksList[index2];

        childTracksList[index1] = childTrack2;
        childTracksList[index2] = childTrack1;

        UpdateChilds();

        swapChildAction(index1, index2);
    }

    private void UpdateSize()
    {
        float height = headHeight + (childTracksList.Count * itemHeight);
        contentRoot.style.height = height;
        menuRoot.style.height = height;

        menuItemParent.style.height = childTracksList.Count * itemHeight;
    }


    //添加子轨道
    private void AddButtonClicked()
    {
        addChildTrackAction?.Invoke();
    }

    public ChildTrack AddChildTrack()
    {
        ChildTrack childTrack = new ChildTrack();
        childTrack.Init(menuItemParent, childTracksList.Count, contentRoot, DeleteChildTrackAndData, DeleteChildTrack, UpdateChildTrackName);
        childTracksList.Add(childTrack);
        UpdateSize();
        return childTrack;
    }

    private void UpdateChildTrackName(ChildTrack childTrack, string newName)
    {
        // 未来可能做其他操作。。

        updateTrackNameAction?.Invoke(childTrack, newName); 
    }

    //删除子轨道显示层面
    private void DeleteChildTrack(ChildTrack childTrack)
    {
        int index = childTrack.GetIndex();
        childTrack.DoDestroy();
        childTracksList.RemoveAt(index);
        //所有的子轨道都需要更新一下索引
        UpdateChilds(index);
        UpdateSize();
    }
    //删除子轨道以及对应数据
    private void DeleteChildTrackAndData(ChildTrack childTrack)
    {
        if (deleteChildTrackFunc == null) return;
        int index = childTrack.GetIndex();
        if (deleteChildTrackFunc(index))
        {
            childTrack.DoDestroy();
            childTracksList.RemoveAt(index);
            //所有的子轨道都需要更新一下索引
            UpdateChilds(index);
            UpdateSize();
        }
    }
    private void UpdateChilds(int startIndex = 0)
    {
        for (int i = startIndex; i < childTracksList.Count; i++)
        {
            childTracksList[i].SetIndex(i);
        }
    }
    public void ClearChildTruck(ChildTrack childTrack)
    {
        int index = childTrack.GetIndex();
        childTrack.Destory();
        childTracksList.RemoveAt(index);
        //所有的子轨道都需要更新一下索引
        UpdateChilds(index);
        UpdateSize();
    }
    /// <summary>
    /// 多行轨道的子轨道
    /// </summary>
    public class ChildTrack
    {
        private const string childTrackMenuAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenuItem.uxml";
        private const string childTrackContentAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackContentItem.uxml";

        public Label titleLabel;
        #region 自身根节点（我自己）
        public VisualElement menuRoot;
        public VisualElement trackRoot;
        #endregion
        #region 自身父节点（放到谁上面）
        public VisualElement menuParent;
        public VisualElement trackParent;
        #endregion


        private TextField trackNameField;
        private Action<ChildTrack> deleteAction;
        private Action<ChildTrack> destroyAction;
        private Action<ChildTrack, string> updateTrackNameAction;

        private static Color normalColor = new Color(0, 0, 0, 0);
        private static Color selectColor = Color.green;

        private int index;
        
        /// <summary>
        /// 初始化子轨道
        /// </summary>
        /// <param name="menuParent">自身根节点（我自己）</param>
        /// <param name="index">索引</param>
        /// <param name="trackParent">要创建到的父轨道</param>
        /// <param name="deleteAction">删除事件</param>
        public void Init(VisualElement menuParent, int index, VisualElement trackParent, Action<ChildTrack> deleteAction, Action<ChildTrack> destroyAction, Action<ChildTrack, string> updateTrackNameAction)
        {
            this.menuParent = menuParent;
            this.trackParent = trackParent;
            this.deleteAction = deleteAction;
            this.destroyAction = destroyAction;
            this.updateTrackNameAction = updateTrackNameAction;

            menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(childTrackMenuAssetPath).Instantiate().Query().ToList()[1];//不要容器，直接持有目标物体
            menuParent.Add(menuRoot);
            trackRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(childTrackContentAssetPath).Instantiate().Query().ToList()[1];//不要容器，直接持有目标物体
            trackParent.Add(trackRoot);

            trackNameField = menuRoot.Q<TextField>("NameField");
            trackNameField.RegisterCallback<FocusInEvent>(TrackNameFileFieldFocusIn);
            trackNameField.RegisterCallback<FocusOutEvent>(TrackNameFileFieldFocusOut);

            Button deleteButton = menuRoot.Q<Button>("DeleteButton");
            deleteButton.clicked += () => deleteAction(this);

            SetIndex(index);
            UnSelect();
        }

        private string oldTrackNameFieldValue;

        private void TrackNameFileFieldFocusIn(FocusInEvent evt)
        {
            oldTrackNameFieldValue = trackNameField.value;
        }


        private void TrackNameFileFieldFocusOut(FocusOutEvent evt)
        {
            if(oldTrackNameFieldValue != trackNameField.value)
            {
                updateTrackNameAction?.Invoke(this, trackNameField.value);
            }
        }

        private VisualElement content;
        public void InitContent(VisualElement content)
        {
            this.content = content;
            trackRoot.Add(content);
        }

        public void SetTrackName(string name)
        {
            trackNameField.value = name;
        }
        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
            float height = 0;
            Vector3 menuPos = menuRoot.transform.position;
            height = index * itemHeight;
            menuPos.y = height;
            menuRoot.transform.position = menuPos;

            Vector3 trackPos = trackRoot.transform.position;
            height = index * itemHeight + headHeight;
            trackPos.y = height;
            trackRoot.transform.position = trackPos;
        }


        public virtual void AddItem(VisualElement ve)
        {
            trackRoot.Add(ve);
        }

        public virtual void DeleteItem(VisualElement ve)
        {   
            trackRoot.Remove(ve);
        }

        public virtual void Destory()
        {
            destroyAction(this);
        }

        public void DoDestroy()
        {   
            if (menuRoot != null) menuParent.Remove(menuRoot);
            if (trackRoot != null) trackParent.Remove(trackRoot);
        }

        public void Select()
        {
            menuRoot.style.backgroundColor = selectColor;
        }
        public void UnSelect()
        {
            menuRoot.style.backgroundColor = normalColor;
        }
    }

}
