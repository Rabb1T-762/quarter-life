using System;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace _Project.Scripts
{
    public class PlayerCharacterController : MonoBehaviour
    {
        private CharacterController controller;
        private InputManager inputManager;
        private PlayerCameraController cameraController;

        [SerializeField] private float playerMaxGroundedSpeed = 10f;
        [SerializeField] private float playerMaxAirborneSpeed = 15f;
        [SerializeField] private float playerWalkSpeedMultiplier = 0.5f;
        [SerializeField] private float playerCrouchSpeedMultiplier = 0.4f;
        [SerializeField] private float playerAcceleration = 100f;
        [SerializeField] private float transformCrouchHeight = 1f;
        [SerializeField] private float transformStandingHeight = 2f;
        [SerializeField] private float heightTransitionSpeed = 10f;

        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.2f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float jumpForceMultiplier = -2.0f;

        [SerializeField] private Weapon mainWeapon;

        private Vector3 playerVelocity;
        private float playerCurrentVelocity;
        private float targetVelocity;
        private float targetTransformHeight;
        private float movementMultiplier = 1f;
        private bool isGrounded;

        private Vector3 lastPosition = Vector3.zero;
        private bool isMoving;

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
            cameraController = GetComponent<PlayerCameraController>();
            inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            Vector3 moveDirection = inputManager.GetMoveInput();
            bool jumpInput = inputManager.GetJumpInput();
            bool walkInput = inputManager.GetWalkInputHeld();
            bool crouchInput = inputManager.GetCrouchInputHeld();
            ProcessMove(moveDirection, jumpInput, walkInput, crouchInput);

            Vector2 lookDirection = inputManager.GetLookInput();
            cameraController.ProcessLook(lookDirection);

            if (inputManager.GetTriggerInputPressed())
            {
                mainWeapon.FireWeapon();
            }
        }

        // receive the inputs from our InputManager.cs and apply them to our character controller
        public void ProcessMove(Vector3 moveDirection, bool jumpInput, bool walkInput, bool crouchInput)
        {
            if (lastPosition != gameObject.transform.position)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        
            // Grounded check
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            // Resetting the player velocity
            movementMultiplier = 1f;
            if (isGrounded && playerVelocity.y < 0)
            {
                // applying constant gravity to the player
                playerVelocity.y = -2f;
            }

            // Handle Walking
            if (walkInput)
            {
                Walk();
            }

            if (crouchInput)
            {
                Crouch();
            }
            else
            {
                targetTransformHeight = transformStandingHeight;
            }

            // Update the character height
            HandleCharacterHeight();

            // calculate target velocity
            targetVelocity = ((isGrounded) ? playerMaxGroundedSpeed : playerMaxAirborneSpeed) * movementMultiplier;

            // calculate the player's current velocity
            if (Math.Abs(playerCurrentVelocity - targetVelocity) > 0.001f)
            {
                playerCurrentVelocity =
                    Mathf.SmoothStep(playerCurrentVelocity, targetVelocity, playerAcceleration * Time.deltaTime);
            }

            var transformMoveDirection = transform.TransformDirection(moveDirection);

            controller.Move(transformMoveDirection * (playerCurrentVelocity * Time.deltaTime));

            // Handle Jumping
            if (isGrounded && jumpInput)
            {
                Jump();
            }

            // Handle Falling
            playerVelocity.y += gravity * Time.deltaTime;

            // Executing the jump
            controller.Move(playerVelocity * Time.deltaTime);


            lastPosition = gameObject.transform.position;
        }

        private void Jump()
        {
            // this is based on solving for initial velocity in the kinematic equation
            // u = sqrt(-2 * gravity * jumpHeight)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * jumpForceMultiplier * gravity);
        }

        private void Walk()
        {
            movementMultiplier *= playerWalkSpeedMultiplier;
        }

        private void Crouch()
        {
            movementMultiplier *= playerCrouchSpeedMultiplier;
            targetTransformHeight = transformCrouchHeight;
        }

        private void HandleCharacterHeight()
        {
            if (Math.Abs(controller.height - targetTransformHeight) > 0.001f)
            {
                controller.height = Mathf.Lerp(controller.height, targetTransformHeight,
                    Time.deltaTime * heightTransitionSpeed);
            }

            // calculate the center of the character controller
            Vector3 playerObjectBottom = controller.center - new Vector3(0, controller.height / 2, 0);

            // set the ground check object to the bottom of the character controller
            groundCheck.transform.localPosition = playerObjectBottom;
        }
    }
}