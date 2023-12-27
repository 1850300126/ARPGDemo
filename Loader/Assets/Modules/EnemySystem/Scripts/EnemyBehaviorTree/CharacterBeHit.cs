    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Unity.VisualScripting;

public class CharacterBeHit : EnemyConditionBase
{   
    public void BeHit()
    {       
        enemy.animator.CrossFade("BeHit", 0.1f);
    }
}
