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
    public EnemyStateMachine enemy_state_machine;
    public Collider find_enemy_collider;
    // Start is called before the first frame update
    void Start()
    {
        enemy_rb = GetComponent<Rigidbody>();

        enemy_collider = GetComponent<Collider>();

        agent = GetComponent<NavMeshAgent>();
        
        animator = GetComponent<Animator>();

        find_enemy_collider = transform.Find("find_enemy_collider").GetComponent<Collider>();

        enemy_state_machine = new EnemyStateMachine(this);

        enemy_state_machine.ChangeState(enemy_state_machine.idle_state);
    }

    // Update is called once per frame
    void Update()
    {
        enemy_state_machine.Update();
    }

    void FixUpdate()
    {
        enemy_state_machine.FixUpdate();
    }

    void OnTriggerEnter(Collider collider)
    {
        enemy_state_machine.OnTriggerEnter(collider);
    }
    void OnTriggerStay(Collider collider)
    {
        enemy_state_machine.OnTriggerStay(collider);
    }
    void OnTriggerExit(Collider collider)
    {
        enemy_state_machine.OnTriggerExit(collider);
    }
}
