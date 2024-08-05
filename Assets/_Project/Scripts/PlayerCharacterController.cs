using System;
using System.Collections;
using NSubstitute.Core;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace _Project.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCharacterController : MonoBehaviour
    {
        private Transform _playerTransform;
        private Rigidbody _rigidbody;
        private InputManager _inputManager;
        private PlayerCameraController _cameraController;

        [Header("Player Visual")] [SerializeField]
        private GameObject playerVisual;

        [SerializeField] private Collider playerVisualCollider;

        [Header("Speed and Acceleration")] [SerializeField]
        private float groundedSpeedLimit = 10f;

        [SerializeField] private float airborneSpeedLimit = 1f;
        [SerializeField] private float groundAcceleration = 100f;
        [SerializeField] private float airAcceleration = 100f;
        [SerializeField] private float friction = 10f;

        [Header("Crouching")] [SerializeField] private float playerCrouchSpeedMultiplier = 0.4f;
        [SerializeField] private float transformCrouchHeight = 1f;
        [SerializeField] private float heightTransitionSpeed = 100f;

        [Header("Walking")] [SerializeField] private float transformStandingHeight = 2f;
        [SerializeField] private float playerWalkSpeedMultiplier = 0.5f;

        [Header("Ground Check")] [SerializeField]
        private Transform groundCheck;

        [SerializeField] private float groundDistance = 0.1f;
        [SerializeField] private LayerMask groundMask;

        [Header("Gravity and Jumping")] [SerializeField]
        private float gravity = -9.8f;

        [SerializeField] private float groundedGravity = -2f;

        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float jumpForceMultiplier = -2.0f;
        [SerializeField] private bool autoJump;
        [SerializeField] private float jumpTimer = 0.1f;

        [Header("Weapons")] [SerializeField] private Weapon mainWeapon;

        private Vector3 _playerVelocity;
        private float _playerCurrentVelocity;
        private float _targetVelocity;
        private float _targetTransformHeight;
        private float _movementMultiplier = 1f;
        private bool _isGrounded;

        private Vector3 _playerMoveInput;
        private Vector2 _playerLookInput;
        private bool _playerWalkInput;
        private bool _playerCrouchInput;
        private bool _playerWeaponTriggerInput;
        private bool _isJumping;

        // Start is called before the first frame update
        void Start()
        {
            _playerTransform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody>();
            _cameraController = GetComponent<PlayerCameraController>();
            _inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            Vector3 moveInput = GetAdjustedMovementInput(_playerTransform, _inputManager);
            _playerLookInput = _inputManager.GetLookInput();
            bool jumpInput = _inputManager.GetJumpInput();
            bool walkInput = _inputManager.GetWalkInputHeld();
            bool crouchInput = _inputManager.GetCrouchInputHeld();

            ProcessMove(moveInput, jumpInput, walkInput, crouchInput);
            ProcessLook();

            if (_inputManager.GetTriggerInputPressed())
            {
                HandleShooting(_inputManager.GetTriggerInputPressed());
            }
        }

        // Return the player input relative to the transform's current forward direction
        private Vector3 GetAdjustedMovementInput(Transform characterTransform, InputManager inputManager)
        {
            var playerInput = inputManager.GetMoveInput();
            var rotation = characterTransform.rotation;

            return rotation * new Vector3(playerInput.x, 0f, playerInput.z).normalized;
        }

        private void ProcessLook()
        {
            _cameraController.ProcessLook(_playerLookInput);
        }

        private void ProcessMove(Vector3 moveInput, bool jumpInput, bool walkInput, bool crouchInput)
        {
            var initialPlayerVelocity = _rigidbody.velocity;
            _movementMultiplier = 1f;

            // don't perform a ground check while jumping 
            // this allows the player to clear the ground before next ground check
            if (!_isJumping)
            {
                _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }

            Vector3 updatedPlayerVelocity;

            if (!_isGrounded)
            {
                updatedPlayerVelocity =
                    Accelerate(moveInput, initialPlayerVelocity, airAcceleration, airborneSpeedLimit);
            }
            else
            {
                if (walkInput)
                {
                    _movementMultiplier = Walk(_movementMultiplier);
                }
                updatedPlayerVelocity = Accelerate(moveInput, initialPlayerVelocity, groundAcceleration,
                    groundedSpeedLimit);
                updatedPlayerVelocity = ApplyFriction(updatedPlayerVelocity);
            }

            updatedPlayerVelocity = ApplyGravity(updatedPlayerVelocity);

            // Handle Jumping
            if (jumpInput && _isGrounded)
            {
                // only jump if the player is not already jumping
                if (!_isJumping)
                {
                    Debug.Log("Jump!");
                    updatedPlayerVelocity = ApplyJump(updatedPlayerVelocity);
                    _isGrounded = false;
                    StartCoroutine(JumpTimer());
                    Debug.Log(updatedPlayerVelocity);
                }
            }

            _rigidbody.velocity = updatedPlayerVelocity;
            Debug.Log("_rigidbody.velocity: " + _rigidbody.velocity);
        }


        private Vector3 Accelerate(
            Vector3 inputDirection,
            Vector3 currentVelocity,
            float accelerationForce,
            float maxVelocity
        )
        {
            // project the current speed onto the max speed
            float speedToAdd = maxVelocity - Vector3.Dot(currentVelocity, inputDirection);

            // There is no more speed to add
            if (speedToAdd <= 0)
            {
                return currentVelocity;
            }

            float desiredVelocity = accelerationForce * Time.deltaTime;

            // clip the acceleration
            if (desiredVelocity > speedToAdd)
            {
                desiredVelocity = speedToAdd;
            }

            return currentVelocity + desiredVelocity * inputDirection;
        }


        private Vector3 ApplyFriction(Vector3 previousVelocity)
        {
            float speed = previousVelocity.magnitude;

            if (speed != 0)
            {
                float drop = speed * friction * Time.deltaTime;
                previousVelocity *= Mathf.Max(speed - drop, 0) / speed;
            }

            return previousVelocity;
        }

        private void HandleShooting(bool isShooting)
        {
            if (isShooting)
            {
                mainWeapon.FireWeapon();
            }
        }

        private Vector3 ApplyJump(Vector3 playerMoveDirection)
        {
            // this is based on solving for initial velocity in the kinematic equation
            // u = sqrt(-2 * gravity * jumpHeight)
            var jumpForce = Mathf.Sqrt(jumpHeight * jumpForceMultiplier * gravity);

            return new Vector3(playerMoveDirection.x, jumpForce, playerMoveDirection.z);
        }

        private IEnumerator JumpTimer()
        {
            _isJumping = true;
            yield return new WaitForSeconds(jumpTimer);
            _isJumping = false;
        }

        private Vector3 ApplyGravity(Vector3 playerMoveDirection)
        {
            if (_isGrounded)
            {
                return new Vector3(playerMoveDirection.x, groundedGravity, playerMoveDirection.z);
            }

            var currentGravity = playerMoveDirection.y;

            var appliedGravity = currentGravity + (gravity * Time.deltaTime);

            return new Vector3(playerMoveDirection.x, appliedGravity, playerMoveDirection.z);
        }

        private float Walk(float movementMultiplier)
        {
            return movementMultiplier * playerWalkSpeedMultiplier;
        }

        private void Crouch()
        {
            _movementMultiplier *= playerCrouchSpeedMultiplier;
            _targetTransformHeight = transformCrouchHeight;
        }

        private void HandleCharacterHeightChange()
        {
            // UpdateCharacterHeight();
            // UpdateGroundCheckPosition();
        }

        // this routine avoids consecutive jumps 
        // and stops the ground check to allow to jump
        private IEnumerator JumpRoutine()
        {
            _isJumping = true;
            yield return new WaitForSeconds(jumpTimer);
            _isJumping = false;
        }

        private void UpdateCharacterHeight()
        {
            if (Math.Abs(_rigidbody.transform.localScale.y - _targetTransformHeight) > 0.001f)
            {
                var scale = _rigidbody.transform.localScale;
                scale.y = Mathf.Lerp(_rigidbody.transform.localScale.y, _targetTransformHeight,
                    Time.deltaTime * heightTransitionSpeed);

                _rigidbody.transform.localScale = scale;
            }
        }

        private void UpdateGroundCheckPosition()
        {
            // calculate the center of the character controller
            Vector3 playerObjectBottom = playerVisualCollider.bounds.center -
                                         new Vector3(0, playerVisualCollider.bounds.extents.y, 0);

            // set the ground check object to the bottom of the character controller
            // playerObjectBottom is calculated in world space so must be converted to local space with 
            // InverseTransformPoint
            groundCheck.transform.localPosition = transform.InverseTransformPoint(playerObjectBottom);
        }
    }
}