using UnityEngine;

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

            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }

        public virtual Vector3 GetMoveInput()
        {
            Vector3 move = new Vector3(
                _onFoot.Movement.ReadValue<Vector2>().x, 0f, 
                _onFoot.Movement.ReadValue<Vector2>().y
            );
            
            return move;
        }
    
        public virtual bool GetJumpInput()
        {
            return _onFoot.Jump.WasPressedThisFrame();
        }

        public virtual bool GetWalkInputHeld()
        {
            return _onFoot.Walk.IsPressed();
        }
    
        public virtual bool GetCrouchInputHeld()
        {
            return _onFoot.Crouch.IsPressed();
        }

        public virtual Vector2 GetLookInput()
        {
            return _onFoot.Look.ReadValue<Vector2>();
        }
    
        public virtual bool GetTriggerInputPressed()
        {
            return _onFoot.Shoot.WasPressedThisFrame();
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