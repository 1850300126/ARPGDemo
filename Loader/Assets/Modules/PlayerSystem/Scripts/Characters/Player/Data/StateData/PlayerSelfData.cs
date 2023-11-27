using System;
using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;
[Serializable]
public class PlayerSelfData
{
    // 体力最大值
    [field: SerializeField] public float max_energy { get; set; } = 100;
    // 体力恢复速率
    [field: SerializeField] public float recharge_speed { get; set; } = 5;
    // 当前体力值
    private float Spirit;
    public float spirit
    {   
        get
        {
            return Spirit;
        }
        set
        {   
            if(value != spirit)
            {
                Spirit = value;
                MsgSystem.instance.SendMsg("spirit_percent", new object[]{ (Spirit / max_energy) });
            }
        } 
    } 

    public void InitPlayerSelfData()
    {
        Spirit = max_energy;
    }
}