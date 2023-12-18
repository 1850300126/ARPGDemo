using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerMovementStateMachine : StateMachine
{
    public PlayerStateReusableData reusable_data { get; }
    public Player player { get; }
    public PlayerIdleState idle_state;
    public PlayerRunState run_state;
    public PlayerLightLandState light_land_state;
    public PlayerDodgeState dodge_state;
    public PlayerSprintState sprint_state;
    public PlayerMoveToTarget move_target_state;


    public PlayerJumpState jump_state;
    public PlayerFallState fall_state;

    public LightStoppingState light_stop_state;
    public HardStoppingState hard_stop_state;

    public AttackIdleState attack_idle_state;
    public LightAttackState light_attack_state;
    public HardAttackState hard_attack_state;
    public AttackFinishState attack_finish_state;
    public LightAttackFinishState light_attack_finish_state;
    public HardAttackFinishState hard_attack_finish_state;

    public PlayerMovementStateMachine(Player player)
    {
        this.player = player;

        reusable_data = new PlayerStateReusableData();

        idle_state = new PlayerIdleState(this);

        run_state = new PlayerRunState(this);

        light_land_state = new PlayerLightLandState(this);

        dodge_state = new PlayerDodgeState(this);

        sprint_state = new PlayerSprintState(this);

        move_target_state = new PlayerMoveToTarget(this);


        jump_state = new PlayerJumpState(this);

        fall_state = new PlayerFallState(this);
        

        light_stop_state = new LightStoppingState(this);

        hard_stop_state = new HardStoppingState(this);


        attack_idle_state = new AttackIdleState(this);

        light_attack_state = new LightAttackState(this);

        hard_attack_state = new HardAttackState(this);

        attack_finish_state = new AttackFinishState(this);

        light_attack_finish_state = new LightAttackFinishState(this);

        hard_attack_finish_state = new HardAttackFinishState(this);
    }
}
