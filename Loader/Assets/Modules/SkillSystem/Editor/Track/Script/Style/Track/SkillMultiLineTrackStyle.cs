using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillMultiLineTrackStyle : SkillTrackStyleBase
{
    #region ����
    private const string menuAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenu.uxml";
    private const string trackAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackContent.uxml";
    private const float headHeight = 35;//5�Ǽ��
    private const float itemHeight = 32;//2�ǵײ���߾�

    #endregion


    private Action addChildTrackAction;
    private Func<int, bool> deleteChildTrackFunc;
    private Action<int, int> swapChildAction;
    private Action<ChildTrack, string> updateTrackNameAction;
    private VisualElement menuItemParent;//�ӹ���Ĳ˵�������
    public List<ChildTrack> childTracksList = new List<ChildTrack>();


    public void Init(VisualElement menuParent, VisualElement contentParent, string title, Action addChildTrackAction, Func<int, bool> deleteChildTrackFunc, Action<int, int> swapChildAction, Action<ChildTrack, string> updateTrackNameAction)
    {
        this.menuParent = menuParent;
        this.contentParent = contentParent;
        this.addChildTrackAction = addChildTrackAction;
        this.deleteChildTrackFunc = deleteChildTrackFunc;
        this.swapChildAction = swapChildAction; 
        this.updateTrackNameAction = updateTrackNameAction;

        menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(menuAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        contentRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        menuParent.Add(menuRoot);
        contentParent.Add(contentRoot);

        titleLabel = menuRoot.Q<Label>("Title");
        titleLabel.text = title;

        menuItemParent = menuRoot.Q<VisualElement>("TrackMenuList");

        menuItemParent.RegisterCallback<MouseDownEvent>(ItemParentMouseDown);
        menuItemParent.RegisterCallback<MouseMoveEvent>(ItemParentMouseMove);
        menuItemParent.RegisterCallback<MouseUpEvent>(ItemParentMouseUp);
        menuItemParent.RegisterCallback<MouseOutEvent>(ItemParentMouseOut);

        //����ӹ���İ�ť
        Button addButton = menuRoot.Q<Button>("AddButton");
        addButton.clicked += AddButtonClicked;

        UpdateSize();
    }

    #region �ӹ����꽻��

    private bool isDragging = false;
    private int selectTrackIndex = -1;

    private void ItemParentMouseDown(MouseDownEvent evt)
    {   
        // �رվɵ�
        if(selectTrackIndex != -1)
        {
            childTracksList[selectTrackIndex].UnSelect();
        }


        // ͨ�����ظ߶��Ƶ�����ǰ�������ǵڼ������
        // �ӹ���ȡֵ��������������������޷�ѡ����ȷ�Ĺ��
        float mousePosition = evt.localMousePosition.y - itemHeight / 2;
        selectTrackIndex = GetChildIndexByMousePosition(mousePosition);
        childTracksList[selectTrackIndex].Select();
        // TODO:��ק
        isDragging = true;
    }
    private void ItemParentMouseMove(MouseMoveEvent evt)
    {
        if(selectTrackIndex == -1 || isDragging == false) return;   

        float mousePosition = evt.localMousePosition.y - itemHeight / 2;
        int mouseTrackIndex = GetChildIndexByMousePosition(mousePosition);

        // �����Լ�����
        if(mouseTrackIndex != selectTrackIndex)
        {   
            SwpChildTrack(selectTrackIndex, mouseTrackIndex);
            // ��ѡ�еĹ������Ϊ��ק�Ĺ��
            selectTrackIndex = mouseTrackIndex;
        }
    }
    private void ItemParentMouseUp(MouseUpEvent evt)
    {
        isDragging = false;
    }
    private void ItemParentMouseOut(MouseOutEvent evt)
    {
        // ItemParentMouseOut����������ã���Ϊ����������ǲ����ڵ�
        // �������Ƿ�����뿪�����ǵļ�ⷶΧ
        if(!menuItemParent.contentRect.Contains(evt.localMousePosition))
        {
            isDragging = false;
        }

    }

    private int GetChildIndexByMousePosition(float mousePositionY)
    {   
        // ������ѡ������ĸ����
        int trackIndex = Mathf.RoundToInt(mousePositionY / itemHeight);
        // ��ȫУ��, ������������count
        trackIndex = Mathf.Clamp(trackIndex, 0, childTracksList.Count - 1);
        return trackIndex;
        
    }
    #endregion

    // �����ӹ��
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


    //����ӹ��
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
        // δ��������������������

        updateTrackNameAction?.Invoke(childTrack, newName); 
    }

    //ɾ���ӹ����ʾ����
    private void DeleteChildTrack(ChildTrack childTrack)
    {
        int index = childTrack.GetIndex();
        childTrack.DoDestroy();
        childTracksList.RemoveAt(index);
        //���е��ӹ������Ҫ����һ������
        UpdateChilds(index);
        UpdateSize();
    }
    //ɾ���ӹ���Լ���Ӧ����
    private void DeleteChildTrackAndData(ChildTrack childTrack)
    {
        if (deleteChildTrackFunc == null) return;
        int index = childTrack.GetIndex();
        if (deleteChildTrackFunc(index))
        {
            childTrack.DoDestroy();
            childTracksList.RemoveAt(index);
            //���е��ӹ������Ҫ����һ������
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
        //���е��ӹ������Ҫ����һ������
        UpdateChilds(index);
        UpdateSize();
    }
    /// <summary>
    /// ���й�����ӹ��
    /// </summary>
    public class ChildTrack
    {
        private const string childTrackMenuAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenuItem.uxml";
        private const string childTrackContentAssetPath = "Assets/Modules/SkillSystem/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackContentItem.uxml";

        public Label titleLabel;
        #region ������ڵ㣨���Լ���
        public VisualElement menuRoot;
        public VisualElement trackRoot;
        #endregion
        #region �����ڵ㣨�ŵ�˭���棩
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
        /// ��ʼ���ӹ��
        /// </summary>
        /// <param name="menuParent">������ڵ㣨���Լ���</param>
        /// <param name="index">����</param>
        /// <param name="trackParent">Ҫ�������ĸ����</param>
        /// <param name="deleteAction">ɾ���¼�</param>
        public void Init(VisualElement menuParent, int index, VisualElement trackParent, Action<ChildTrack> deleteAction, Action<ChildTrack> destroyAction, Action<ChildTrack, string> updateTrackNameAction)
        {
            this.menuParent = menuParent;
            this.trackParent = trackParent;
            this.deleteAction = deleteAction;
            this.destroyAction = destroyAction;
            this.updateTrackNameAction = updateTrackNameAction;

            menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(childTrackMenuAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
            menuParent.Add(menuRoot);
            trackRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(childTrackContentAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
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
