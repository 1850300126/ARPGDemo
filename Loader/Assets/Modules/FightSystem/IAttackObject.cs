using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AttackObjectType
{
    BeAttacked,
    Enemy,
    NoAttack
}

public interface IAttackObject
{
    public AttackObjectType SelfType {get; set;}

    public AttackObjectType AttackType {get; set;}

    public void BeHit();
}
