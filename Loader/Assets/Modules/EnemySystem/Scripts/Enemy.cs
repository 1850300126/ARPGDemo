using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IAttackObject
{   
    public Rigidbody enemy_rb;
    public Collider enemy_collider;
    public NavMeshAgent agent;
    public Animator animator;   

    public BehaviorTree behavior_tree;

    [Header("数据类")]
    [Header("是否允许巡逻")]
    public bool patrol;
    [Header("是否在攻击中")]
    public bool attacking;

    public AttackObjectType self_type = AttackObjectType.Enemy;
    public AttackObjectType SelfType { get => self_type; set => self_type = value ; }
    public AttackObjectType attack_type = AttackObjectType.BeAttacked;
    public AttackObjectType AttackType { get => attack_type; set => attack_type = value; }

    void Start()
    {
        enemy_rb = GetComponent<Rigidbody>();

        enemy_collider = GetComponent<Collider>();

        agent = GetComponent<NavMeshAgent>();
        
        animator = GetComponent<Animator>();

        behavior_tree = GetComponent<BehaviorTree>();
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, 5);
    }

    public virtual void BeHit()
    {   
        Debug.Log("被击中了");
    }
}
