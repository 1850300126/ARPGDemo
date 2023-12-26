using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{   
    public BoxCollider attack_collider;
    private void Start() 
    {
        InitWeapon();    
    }

    public void InitWeapon()
    {
        attack_collider = this.GetComponent<BoxCollider>();
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
        IAttackObject attack_object = collider.gameObject.GetComponent<IAttackObject>();

        if(attack_object == null) return;

        attack_object.BeHit();

        Debug.Log("打中敌人");
    }

    public void OpenCollider()
    {
        attack_collider.enabled = true;
    }
    public void CloseCollider(object param)
    {
        attack_collider.enabled = false;
    }
}
