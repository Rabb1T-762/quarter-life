using System;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace _Project.Scripts
{
    public class PlayerCharacterController : MonoBehaviour
    {
        private CharacterController _controller;
        private InputManager _inputManager;
        private PlayerCameraController _cameraController;

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

        private Vector3 _playerVelocity;
        private float _playerCurrentVelocity;
        private float _targetVelocity;
        private float _targetTransformHeight;
        private float _movementMultiplier = 1f;
        private bool _isGrounded;

        private Vector3 _lastPosition = Vector3.zero;
        private bool _isMoving;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _cameraController = GetComponent<PlayerCameraController>();
            _inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            Vector3 moveDirection = _inputManager.GetMoveInput();
            bool jumpInput = _inputManager.GetJumpInput();
            bool walkInput = _inputManager.GetWalkInputHeld();
            bool crouchInput = _inputManager.GetCrouchInputHeld();
            ProcessMove(moveDirection, jumpInput, walkInput, crouchInput);

            Vector2 lookDirection = _inputManager.GetLookInput();
            _cameraController.ProcessLook(lookDirection);

            if (_inputManager.GetTriggerInputPressed())
            {
                mainWeapon.FireWeapon();
            }
        }

        // receive the inputs from our InputManager.cs and apply them to our character controller
        private void ProcessMove(Vector3 moveDirection, bool jumpInput, bool walkInput, bool crouchInput)
        {
            _isMoving = _lastPosition != gameObject.transform.position;

            // Grounded check
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            // Resetting the player velocity
            _movementMultiplier = 1f;
            if (_isGrounded && _playerVelocity.y < 0)
            {
                // applying constant gravity to the player
                _playerVelocity.y = -2f;
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
                _targetTransformHeight = transformStandingHeight;
            }

            // Update the character height
            HandleCharacterHeightChange();

            // calculate target velocity
            _targetVelocity = ((_isGrounded) ? playerMaxGroundedSpeed : playerMaxAirborneSpeed) * _movementMultiplier;

            // calculate the player's current velocity
            if (Math.Abs(_playerCurrentVelocity - _targetVelocity) > 0.001f)
            {
                _playerCurrentVelocity =
                    Mathf.SmoothStep(_playerCurrentVelocity, _targetVelocity, playerAcceleration * Time.deltaTime);
            }

            var transformMoveDirection = transform.TransformDirection(moveDirection);

            _controller.Move(transformMoveDirection * (_playerCurrentVelocity * Time.deltaTime));

            // Handle Jumping
            if (_isGrounded && jumpInput)
            {
                Jump();
            }

            // Handle Falling
            _playerVelocity.y += gravity * Time.deltaTime;

            // Executing the jump
            _controller.Move(_playerVelocity * Time.deltaTime);

            _lastPosition = gameObject.transform.position;
        }

        private void Jump()
        {
            // this is based on solving for initial velocity in the kinematic equation
            // u = sqrt(-2 * gravity * jumpHeight)
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * jumpForceMultiplier * gravity);
        }

        private void Walk()
        {
            _movementMultiplier *= playerWalkSpeedMultiplier;
        }

        private void Crouch()
        {
            _movementMultiplier *= playerCrouchSpeedMultiplier;
            _targetTransformHeight = transformCrouchHeight;
        }

        private void HandleCharacterHeightChange()
        {
            UpdateCharacterHeight();
            UpdateGroundCheckPosition();
        }

        private void UpdateCharacterHeight()
        {
            if (Math.Abs(_controller.height - _targetTransformHeight) > 0.001f)
            {
                _controller.height = Mathf.Lerp(_controller.height, _targetTransformHeight,
                    Time.deltaTime * heightTransitionSpeed);
            }
        }

        private void UpdateGroundCheckPosition()
        {
            // calculate the center of the character controller
            Vector3 playerObjectBottom = _controller.center - new Vector3(0, _controller.height / 2, 0);

            // set the ground check object to the bottom of the character controller
            groundCheck.transform.localPosition = playerObjectBottom;
        }
    }
}