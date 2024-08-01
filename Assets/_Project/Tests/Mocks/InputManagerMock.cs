using _Project.Scripts;
using UnityEngine;

namespace _Project.Tests.Mocks
{
    public class InputManagerMock : InputManager
    {
        private Vector3 _moveInput;
        private Vector2 _lookInput;
        private bool _jumpInputPressed;
        private bool _walkInputHeld;
        private bool _crouchInputHeld;
        private bool _triggerInputPressed;

        public void SetMoveInput(Vector3 input) => _moveInput = input;
        public void SetLookInput(Vector2 input) => _lookInput = input;
        public void SetJumpInput(bool input) => _jumpInputPressed = input;
        public void SetWalkInput(bool input) => _walkInputHeld = input;
        public void SetCrouchInput(bool input) => _walkInputHeld = input;
        public void SetTriggerInputPressed(bool input) => _triggerInputPressed = input;

        public override Vector3 GetMoveInput() => _moveInput;
        public override bool GetJumpInput() => _jumpInputPressed;
        public override bool GetWalkInputHeld() => _walkInputHeld;
        public override bool GetCrouchInputHeld() => _crouchInputHeld;
        public override Vector2 GetLookInput() => _lookInput;
        public override bool GetTriggerInputPressed() => _triggerInputPressed;
    }
}