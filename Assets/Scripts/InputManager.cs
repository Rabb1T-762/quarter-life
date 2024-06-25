using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        playerMotor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => playerMotor.Jump();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // make playermoter move using value from our movement action
        playerMotor.ProcessMove(
            onFoot.Movement.ReadValue<Vector2>(),
            onFoot.Jump.ReadValue<float>(),
            onFoot.Walk.ReadValue<float>(),
            onFoot.Crouch.ReadValue<float>()
            );
        playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}