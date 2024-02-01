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
}
