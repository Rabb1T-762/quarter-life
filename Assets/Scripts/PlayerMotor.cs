using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float playerRunSpeed = 8f;
    [SerializeField] private float playerWalkSpeed = 5f;
    [SerializeField] private float playerCrouchSpeed = 4f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float transformCrouchHeight = 1f;
    [SerializeField] private float transformStandingHeight = 2f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float jumpForceMultiplier = -2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    // receive the inputs from our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        
        controller.Move(transform.TransformDirection(moveDirection) * playerSpeed * Time.deltaTime);
        
        // apply constant gravity to player
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            // this is based on solving for initial velocity in the kinematic equation
            // u = sqrt(-2 * gravity * jumpHeight)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * jumpForceMultiplier * gravity);
        }
    }

    public void Walk(float walkButtonPressed)
    {
        if (walkButtonPressed > 0)
        {
            playerSpeed = playerWalkSpeed;
            return;
        }
        
        playerSpeed = playerRunSpeed;
    }

    public void Crouch(float crouchButtonPressed)
    {
        if (crouchButtonPressed > 0)
        {
            controller.height = Mathf.Lerp(controller.height, transformCrouchHeight, crouchTransitionSpeed);
            playerSpeed = playerCrouchSpeed;
            return;
        }

        controller.height = Mathf.Lerp(controller.height, transformStandingHeight, crouchTransitionSpeed);
        playerSpeed = playerRunSpeed;
    }
}
