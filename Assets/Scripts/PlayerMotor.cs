using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private float playerMaxSpeed = 10f;
    [SerializeField] private float playerRunSpeed = 8f;
    [SerializeField] private float playerWalkSpeed = 5f;
    [SerializeField] private float playerCrouchSpeed = 4f;
    [SerializeField] private float playerAcceleration = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float transformCrouchHeight = 1f;
    [SerializeField] private float transformStandingHeight = 2f;
    [SerializeField] private float crouchTransitionSpeed = 10f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private float jumpForceMultiplier = -2.0f;

    private Vector3 playerVelocity;
    private bool isGrounded;

    private Vector3 lastPosition = Vector3.zero;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // receive the inputs from our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input, float jumpInput, float walkInput, float crouchInput)
    {
        // Grounded check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Resetting the player velocity
        if (isGrounded && playerVelocity.y < 0)
        {
            // applying constant gravity to the player
            playerVelocity.y = -2f;
        }

        // Getting the input directions and creating move vector
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        
        var transformMoveDirection = transform.TransformDirection(moveDirection);

        controller.Move(transformMoveDirection * (playerMaxSpeed * Time.deltaTime));
        
        // Handle Jumping
        if (isGrounded && jumpInput > 0)
        {
            Jump();
        }
        
        // Handle Falling
        playerVelocity.y += gravity * Time.deltaTime;
        
        // Executing the jump
        controller.Move(playerVelocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        lastPosition = gameObject.transform.position;
    }

    public void Jump()
    {
            // this is based on solving for initial velocity in the kinematic equation
            // u = sqrt(-2 * gravity * jumpHeight)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * jumpForceMultiplier * gravity);
    }

    public void Walk()
    {
        playerMaxSpeed = playerWalkSpeed;
        Debug.Log("Walking: Player Speed: " + playerMaxSpeed);

        playerMaxSpeed = playerRunSpeed;
        Debug.Log("End Walking: Player Speed: " + playerMaxSpeed);
    }

    public void Crouch()
    {
        {
            controller.height = Mathf.Lerp(controller.height, transformCrouchHeight, crouchTransitionSpeed);
            playerMaxSpeed = playerCrouchSpeed;
            Debug.Log("Crouching: Player Speed: " + playerMaxSpeed);
            return;
        }

        controller.height = Mathf.Lerp(controller.height, transformStandingHeight, crouchTransitionSpeed);
        playerMaxSpeed = playerRunSpeed;
        Debug.Log("Standing: Player Speed: " + playerMaxSpeed);
    }
}