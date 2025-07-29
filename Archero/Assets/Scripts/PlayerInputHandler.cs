using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool DashHeld { get; private set; }

    private PlayerInputControl inputControl;

    private void Awake()
    {
        inputControl = new PlayerInputControl();

        inputControl.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputControl.Player.Move.canceled += _ => MoveInput = Vector2.zero;

        inputControl.Player.Dash.performed += _ => DashHeld = true;
        inputControl.Player.Dash.canceled += _ => DashHeld = false;
    }

    private void OnEnable() => inputControl.Enable();
    private void OnDisable() => inputControl.Disable();
}

