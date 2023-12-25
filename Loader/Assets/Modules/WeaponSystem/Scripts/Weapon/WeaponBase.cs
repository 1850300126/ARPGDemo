using System.Collections;
using System.Collections.Generic;
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

        // CloseCollider();
    }

    private void OnTriggerEnter(Collider collider) 
    {   
        IAttackObject attack_object = collider.gameObject.GetComponent<IAttackObject>();

        if(attack_object == null) return;

        attack_object.BeHit();
    }

    public void OpenCollider()
    {
        attack_collider.enabled = true;
    }
    public void CloseCollider()
    {
        attack_collider.enabled = false;
    }
}
