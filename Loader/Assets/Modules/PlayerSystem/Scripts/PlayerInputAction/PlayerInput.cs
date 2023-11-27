using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public InputActionComponent input_action_comp;
    public InputActionComponent.PlayerInputActionActions player_actions;
    private void Awake() 
    {
        input_action_comp = new InputActionComponent();

        player_actions = input_action_comp.PlayerInputAction;
    }
    private void OnEnable() 
    {
        player_actions.Enable();
    }
    private void OnDisable() 
    {
        player_actions.Disable();
    }
}
