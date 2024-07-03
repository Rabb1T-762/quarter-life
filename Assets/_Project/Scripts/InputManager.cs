using UnityEngine;

namespace _Project.Scripts
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerInput.OnFootActions onFoot;

        void Awake()
        {
            playerInput = new PlayerInput();
            onFoot = playerInput.OnFoot;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public Vector3 GetMoveInput()
        {
            Vector3 move = new Vector3(onFoot.Movement.ReadValue<Vector2>().x , 0f, onFoot.Movement.ReadValue<Vector2>().y);
            return move;
        }
    
        public bool GetJumpInput()
        {
            return onFoot.Jump.WasPressedThisFrame();
        }

        public bool GetWalkInputHeld()
        {
            return onFoot.Walk.IsPressed();
        }
    
        public bool GetCrouchInputHeld()
        {
            return onFoot.Crouch.IsPressed();
        }

        public Vector2 GetLookInput()
        {
            return onFoot.Look.ReadValue<Vector2>();
        }
    
        public bool GetTriggerInputPressed()
        {
            return onFoot.Shoot.WasPressedThisFrame();
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
}