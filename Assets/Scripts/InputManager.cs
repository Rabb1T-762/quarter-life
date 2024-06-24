using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    void FixedUpdate()
    {
       // make playermoter move using value from our movement action
       playerMotor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
       playerMotor.Walk(onFoot.Walk.ReadValue<float>());
       playerMotor.Crouch(onFoot.Crouch.ReadValue<float>());
    }

    private void LateUpdate()
    {
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
