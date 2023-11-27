using System.Collections;
using System.Collections.Generic;
using EasyUpdateDemoSDK;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IAnimationEvent
{   
    // 状态机
    public PlayerMovementStateMachine movement_state_machine;
    // 碰撞体
    public Collider player_collider;
    // 刚体
    public Rigidbody player_rb;
    // 动画器
    public Animator animator;
    public AnimationEventTrigger animator_event_trigger;
    public PlayerInput player_input;
    public Transform cam_trans;
    public Transform hand_point;
    [field: SerializeField] public PlayerAnimationData animation_data { get; private set; }
    [field: SerializeField] public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }
    [field: SerializeField] public PlayerSO player_data;
    [field: SerializeField] public PlayerLayerData layer_data;
    [field: SerializeField] public ComboConfig cureent_weapon;

    private void Awake() 
    {  

    }
    public void OnLoaded()
    {   

        player_data.self_data.InitPlayerSelfData();

        player_collider = this.GetComponentInChildren<Collider>();

        player_rb = GetComponent<Rigidbody>();

        animator = this.GetComponentInChildren<Animator>();
        animation_data = new PlayerAnimationData();
        animation_data.Initialize();
        animator_event_trigger = animator.AddComponent<AnimationEventTrigger>();
        animator_event_trigger.InitEventTrigger(this);


        player_input = this.AddComponent<PlayerInput>();

        ResizableCapsuleCollider = this.GetComponent<PlayerResizableCapsuleCollider>();

        cam_trans = Camera.main.transform;

        layer_data = new PlayerLayerData
        {
            GroundLayer = 1 << LayerMask.NameToLayer("Environment")
        };

        movement_state_machine = new PlayerMovementStateMachine(this);
        movement_state_machine.ChangeState(movement_state_machine.idle_state);
    }
    private void Start() 
    {
        
    }        
    private void Update()
    {
        movement_state_machine.HandleInput();

        movement_state_machine.Update();
    }

    private void FixedUpdate()
    {
        movement_state_machine.PhysicsUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        movement_state_machine.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        movement_state_machine.OnTriggerExit(collider);
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        movement_state_machine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        movement_state_machine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        movement_state_machine.OnAnimationTransitionEvent();
    }

}
