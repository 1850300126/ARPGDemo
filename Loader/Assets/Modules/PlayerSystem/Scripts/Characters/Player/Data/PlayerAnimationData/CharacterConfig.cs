using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig",menuName = "Config/CharacterConfig")]
public class CharacterConfig : ConfigBase
{
    [LabelText("为移动应用RootMotion")] public bool ApplyRootMotionForMove;
    [LabelText("标准动画表")] public Dictionary<string, AnimationClip> StandAnimationDic;

    public AnimationClip GetAnimationByName(string animationName)
    {
        return StandAnimationDic[animationName];
    }
}
