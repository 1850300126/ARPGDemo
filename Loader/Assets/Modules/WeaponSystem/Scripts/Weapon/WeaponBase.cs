using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{   
    public BoxCollider attack_collider;
    // 武器是否可以造成伤害
    public bool weapon_damage;
    private void Start() 
    {
        InitWeapon();    
    }

    public void InitWeapon()
    {
        attack_collider = this.GetComponent<BoxCollider>();
        weapon_damage = false;
    }

    private void OnEnable() 
    {
        MsgSystem.instance.RegistMsgAction("AttackExit", CloseCollider);
    }

    private void OnDisable() 
    {
        MsgSystem.instance.RemoveMsgAction("AttackExit", CloseCollider);
    }

    private void OnTriggerEnter(Collider collider) 
    {   
        if(!weapon_damage) return;

        IAttackObject attack_object = collider.gameObject.GetComponent<IAttackObject>();

        if(attack_object == null) return;

        attack_object.BeHit();

        Debug.Log("打中敌人");
    }

    public void OpenCollider()
    {
        weapon_damage = true;
    }
    public void CloseCollider(object param)
    {
        weapon_damage = false;
    }
}
