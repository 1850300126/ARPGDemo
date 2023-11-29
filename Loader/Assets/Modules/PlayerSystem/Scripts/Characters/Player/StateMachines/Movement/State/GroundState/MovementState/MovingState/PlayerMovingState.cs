using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            StartAnimation(movement_state_machine.player.animation_data.MovingParameterHash);
            
        }

        public override void OnExit()
        {
            base.OnExit();

            StopAnimation(movement_state_machine.player.animation_data.MovingParameterHash);
        }

    protected override void AddInputAction()
    {
        base.AddInputAction();

        movement_state_machine.player.player_input.player_actions.HardAttack.started += OnHardAttackStarted;

        movement_state_machine.player.player_input.player_actions.LightAttack.started += OnLightAttackStarted;

        movement_state_machine.player.player_input.player_actions.Dodge.started += OnDodgeStarted;
    }

    protected override void RemoveInputAction()
    {
        base.RemoveInputAction();

        movement_state_machine.player.player_input.player_actions.HardAttack.started -= OnHardAttackStarted;

        movement_state_machine.player.player_input.player_actions.LightAttack.started -= OnLightAttackStarted;

        movement_state_machine.player.player_input.player_actions.Dodge.started -= OnDodgeStarted;
    }

    protected void OnStop(IState state)
    {
        movement_state_machine.ChangeState(state);
    }
}
