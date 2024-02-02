using System;
using EasyUpdateDemoSDK;
using Taco.Timeline;
using Unity.VisualScripting;
using UnityEngine;
public class Player : WorldObjectBase, IAnimationEvent
{   
    [Header("组件类")]
    // 状态机
    public PlayerMovementStateMachine movement_state_machine;
    // 碰撞体
    public Collider player_collider;
    // 刚体
    public Rigidbody player_rb;
    // 动画器
    public Animator animator;
    // 动画控制器
    [SerializeField] AnimationController animationController;
    public AnimationController AnimationController { get => animationController; } 
    // 技能控制器   
    [SerializeField] private SkillController skillController;
    public SkillController SkillController { get => skillController; }
    // 动画事件管理
    public AnimationEventTrigger animator_event_trigger;
    // 输入组件
    public PlayerInput player_input;
    public Transform cam_trans;
    public Transform hand_point;
    public WeaponBase current_weapon;


    [Header("数据类")]
    [field: SerializeField] public PlayerSO player_data;
    [field: SerializeField] public PlayerLayerData layer_data;
    [field: SerializeField] public TimelineSkillConfig currentSkillConfig;
    // [field: SerializeField] public WeaponAnimationConfigs currentWeaponAnimationConfigs;
    [field: SerializeField] public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }

    public void OnLoaded()
    {   
        player_data.self_data.InitPlayerSelfData();

        player_rb = GetComponent<Rigidbody>();

        player_collider = this.GetComponentInChildren<Collider>();
        ResizableCapsuleCollider = this.GetComponent<PlayerResizableCapsuleCollider>();

        animator = this.GetComponentInChildren<Animator>();
        animator_event_trigger = animator.AddComponent<AnimationEventTrigger>();
        animator_event_trigger.InitEventTrigger(this);

        animationController = animator.GetComponent<AnimationController>();
        animationController.Init();

        skillController = animator.GetComponent<SkillController>();

        player_input = this.AddComponent<PlayerInput>();


        cam_trans = Camera.main.transform;

        layer_data = new PlayerLayerData
        {
            GroundLayer = 1 << LayerMask.NameToLayer("Environment"),
            AttackLayer = 1 << LayerMask.NameToLayer("Enemy")
        };

        hand_point = this.GetComponent<CommonInfo>().GetPoint("right_hand").transform;
    
        movement_state_machine = new PlayerMovementStateMachine(this);
        movement_state_machine.ChangeState(movement_state_machine.idle_state);


        // currentWeaponAnimationConfigs = (WeaponAnimationConfigs)APISystem.instance.CallAPI("weapon_system", "get_combo_config", new object[]{"Katana"});
        
        current_weapon = (WeaponBase)APISystem.instance.CallAPI("weapon_system", "GetWeapon", new object[]{"Katana"});
        current_weapon.transform.parent = hand_point.transform;
        current_weapon.transform.localPosition = Vector3.zero;
        current_weapon.transform.localRotation = Quaternion.Euler(0, 0, -90);
    }
    private void Update()
    {
        movement_state_machine.HandleInput();

        movement_state_machine.Update();
    }

    private void FixedUpdate()
    {
        movement_state_machine.FixUpdate();
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
    /// <summary>
    /// 播放动画
    /// </summary>
    public void PlayAnimation(string animationClipName, Action<Vector3, Quaternion> rootMotionAction = null, float transitionFixedTime = 0.25f)
    {   
        animationController.SetRootMotionAction(rootMotionAction);
        skillController.CtrlPlayable.CrossFade(animationClipName, 0.15f, 0);
        
    }

    /// <summary>
    /// 播放技能
    /// </summary>
    public void PlaySkill(Timeline timeline, Action<Vector3, Quaternion> rootMotionAction = null, Action OnDone = null)
    {
        animationController.SetRootMotionAction(rootMotionAction);
        skillController.PlayTimeline(timeline, OnDone);
    } 

    public override void BeHit()
    {
        Debug.Log("被击中了" + this.name);
    }
}
