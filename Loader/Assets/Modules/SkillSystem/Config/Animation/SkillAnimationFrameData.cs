using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;
/// <summary>
/// ���ܶ�������
/// </summary>
[Serializable]
public class SkillAnimationFrameData
{
    /// <summary>
    /// ����֡����
    /// key:��ʼ֡��
    /// value���¼�����
    /// </summary>
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    [DictionaryDrawerSettings(KeyLabel = "��ʼ֡", ValueLabel = "��������")]
    public Dictionary<int, SkillAnimationClipData> FrameDataDic = new Dictionary<int, SkillAnimationClipData>();
}
