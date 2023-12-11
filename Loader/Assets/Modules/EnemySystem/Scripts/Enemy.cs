using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{   
    public Rigidbody enemy_rb;
    public Collider enemy_collider;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("数据类")]
    [Header("是否允许巡逻")]
    public bool patrol;
    [Header("是否在攻击中")]
    public bool attacking;
    void Start()
    {
        enemy_rb = GetComponent<Rigidbody>();

        enemy_collider = GetComponent<Collider>();

        agent = GetComponent<NavMeshAgent>();
        
        animator = GetComponent<Animator>();
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, 5);
    }
}
