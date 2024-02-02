using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : WorldObjectBase, IAnimationEvent
{   
    public Rigidbody enemy_rb;
    public Collider enemy_collider;
    public NavMeshAgent agent;
    public Animator animator;   
    public BehaviorTree behavior_tree;
    public AnimationController animationController;
    public EnemyStateMachine enemyStateMachine;


    void Start()
    {   
        enemy_rb = GetComponent<Rigidbody>();

        enemy_collider = GetComponent<Collider>();

        agent = GetComponent<NavMeshAgent>();
        
        animator = GetComponent<Animator>();

        behavior_tree = GetComponent<BehaviorTree>();

        animationController = animator.GetComponent<AnimationController>();
        animationController.Init();

        enemyStateMachine = new EnemyStateMachine(this);
        enemyStateMachine.ChangeState(enemyStateMachine.idle_state);
    }    
    private void Update()
    {
        enemyStateMachine.HandleInput();

        enemyStateMachine.Update();
    }

    private void FixedUpdate()
    {
        enemyStateMachine.FixUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        enemyStateMachine.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        enemyStateMachine.OnTriggerExit(collider);
    }
    public void OnMovementStateAnimationEnterEvent()
    {
        enemyStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        enemyStateMachine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        enemyStateMachine.OnAnimationTransitionEvent();
    }
    public override void BeHit()
    {
        enemyStateMachine.ChangeState(enemyStateMachine.behitState);
    }


    private void OnDrawGizmos() 
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, 5);
    }


}
