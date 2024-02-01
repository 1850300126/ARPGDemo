using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

using Custom.Animation;
public class Enemy : WorldObjectBase, IAnimationEvent
{   
    public Rigidbody enemy_rb;
    public Collider enemy_collider;
    public NavMeshAgent agent;
    public Animator animator;   
    public BehaviorTree behavior_tree;
    public AnimationController animationController;
    public EnemyStateMachine enemyStateMachine;

    [Header("鏁版嵁绫?")]

    [field: SerializeField] public CharacterConfig movementAnimationSO;

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
    /// <summary>
    /// 鎾?鏀惧姩鐢?
    /// </summary>
    public void PlayAnimation(string animationClipName, Action<Vector3, Quaternion> rootMotionAction = null, float speed = 1, bool refreshAnimation = false, float transitionFixedTime = 0.25f)
    {   
        animationController.SetRootMotionAction(rootMotionAction);
        animationController.PlaySingleAniamtion(movementAnimationSO.GetAnimationByName(animationClipName), speed, refreshAnimation, transitionFixedTime);
    }

    /// <summary>
    /// 鎾?鏀炬贩鍚堝姩鐢?
    /// </summary>
    public void PlayBlendAnimation(string clip1Name, string clip2Name, Action<Vector3, Quaternion> rootMotionAction = null, float speed = 1, float transitionFixedTime = 0.25f)
    {
        animationController.SetRootMotionAction(rootMotionAction);
        AnimationClip clip1 = movementAnimationSO.GetAnimationByName(clip1Name);
        AnimationClip clip2 = movementAnimationSO.GetAnimationByName(clip2Name);
        animationController.PlayBlendAnimation(clip1, clip2, speed, transitionFixedTime);
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
