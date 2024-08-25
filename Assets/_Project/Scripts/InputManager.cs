using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private PlayerInput.OnFootActions _onFoot;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _onFoot = _playerInput.OnFoot;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public Vector3 GetMoveInput()
        {
            Vector3 move = new Vector3(_onFoot.Movement.ReadValue<Vector2>().x , 0f, _onFoot.Movement.ReadValue<Vector2>().y);
            return move;
        }
    
        public bool GetJumpInput()
        {
            return _onFoot.Jump.WasPressedThisFrame();
        }

        public bool GetWalkInputHeld()
        {
            return _onFoot.Walk.IsPressed();
        }
    
        public bool GetCrouchInputHeld()
        {
            return _onFoot.Crouch.IsPressed();
        }

        public Vector2 GetLookInput()
        {
            return _onFoot.Look.ReadValue<Vector2>();
        }
    
        public bool GetTriggerInputPressed()
        {
            return _onFoot.Shoot.WasPressedThisFrame();
        }

        public bool GetInteractInputPressed()
        {
            return _onFoot.Interact.WasPressedThisFrame();
        }

        private void OnEnable()
        {
            _onFoot.Enable();
        }

        private void OnDisable()
        {
            _onFoot.Disable();
        }
    }
}