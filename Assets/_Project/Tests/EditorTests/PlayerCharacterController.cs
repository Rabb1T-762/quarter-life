using System.Collections;
using System.Reflection;
using _Project.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace _Project.Tests.EditorTests
{
    public class PlayerCharacterControllerTests
    {
        private GameObject _player;
        private PlayerCharacterController _playerController;
        private CharacterController _characterController;
        private InputManager _inputManager;
        private PlayerCameraController _cameraController;
        private Weapon _weapon;
        private Transform _groundCheck;

        [SetUp]
        public void SetUp()
        {
            _player = new GameObject();
            _playerController = _player.AddComponent<PlayerCharacterController>();
            _characterController = _player.AddComponent<CharacterController>();
            _inputManager = _player.AddComponent<InputManager>();
            _cameraController = _player.AddComponent<PlayerCameraController>();
            _weapon = _player.AddComponent<Weapon>();
                
            GameObject groundCheckObject = new GameObject("GroundCheck");
            _groundCheck = groundCheckObject.transform;
            _groundCheck.position = Vector3.zero;
            
            SetPrivateField("groundCheck", _playerController, _groundCheck);
            SetPrivateField("_controller", _playerController, _characterController);
            SetPrivateField("_inputManager", _playerController, _inputManager);
            SetPrivateField("_cameraController", _playerController, _cameraController);
            SetPrivateField("mainWeapon", _playerController, _weapon);

            SetPrivateSerializedField(_playerController, "playerMaxGroundedSpeed", 10f);
            SetPrivateSerializedField(_playerController, "playerMaxAirborneSpeed", 15f);
            SetPrivateSerializedField(_playerController, "playerWalkSpeedMultiplier", 0.5f);
            SetPrivateSerializedField(_playerController, "playerCrouchSpeedMultiplier", 0.4f);
            SetPrivateSerializedField(_playerController, "playerAcceleration", 100f);
            SetPrivateSerializedField(_playerController, "transformCrouchHeight", 1f);
            SetPrivateSerializedField(_playerController, "transformStandingHeight", 2f);
            SetPrivateSerializedField(_playerController, "heightTransitionSpeed", 10f);
            SetPrivateSerializedField(_playerController, "groundDistance", 0.2f);
            SetPrivateSerializedField(_playerController, "groundMask", LayerMask.GetMask("Default"));
            SetPrivateSerializedField(_playerController, "gravity", -9.8f);
            SetPrivateSerializedField(_playerController, "jumpHeight", 2f);
            SetPrivateSerializedField(_playerController, "jumpForceMultiplier", -2.0f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_player);
        }

        [Test]
        public void ProcessMove_Grounded_PlayerVelocityReset()
        {
            // Arrange
            SetPrivateField("_playerVelocity", _playerController, new Vector3(0, -5, 0));

            // Mock Physics.CheckSphere to always return true for this test
            
            // Act
            InvokePrivateMethod(_playerController, "ProcessMove", new object[] {Vector3.zero, false, false, false});

            // Assert
            Vector3 playerVelocity = (Vector3)GetPrivateField("_playerVelocity", _playerController);
            Assert.AreEqual(-2f, playerVelocity.y);
        }

        [Test]
        public void Walk_MultipliesMovementMultiplierByWalkSpeed()
        {
            // Act
            InvokePrivateMethod(_playerController, "Walk", null);

            // Assert
            float movementMultiplier = (float)GetPrivateField("_movementMultiplier", _playerController);
            Assert.AreEqual(0.5f, movementMultiplier);
        }

        [Test]
        public void Crouch_MultipliesMovementMultiplierByCrouchSpeed()
        {
            // Act
            InvokePrivateMethod(_playerController, "Crouch", null);

            // Assert
            float movementMultiplier = (float)GetPrivateField("_movementMultiplier", _playerController);
            float targetTransformHeight = (float)GetPrivateField("_targetTransformHeight", _playerController);
            Assert.AreEqual(0.4f, movementMultiplier);
            Assert.AreEqual(1f, targetTransformHeight);
        }

        [Test]
        public void Jump_SetsPlayerVelocityYCorrectly()
        {
            // Act
            InvokePrivateMethod(_playerController, "Jump", null);

            // Assert
            Vector3 playerVelocity = (Vector3)GetPrivateField("_playerVelocity", _playerController);
            Assert.AreEqual(Mathf.Sqrt(2f * -2.0f * -9.8f), playerVelocity.y);
        }

        [UnityTest]
        public IEnumerator HandleCharacterHeight_ChangesHeightCorrectly()
        {
            // Arrange
            _characterController.height = 2f;
            SetPrivateField("_targetTransformHeight", _playerController, 1f);

            // Act
            yield return null; // Wait for a frame to allow height change

            // Assert
            Assert.AreEqual(1f, _characterController.height, 0.1f);
        }

        private void SetPrivateField(string fieldName, object target, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(target, value);
        }

        private void SetPrivateSerializedField(object target, string fieldName, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            field.SetValue(target, value);
        }

        private object GetPrivateField(string fieldName, object target)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(target);
        }
        
        private void InvokePrivateMethod(object target, string methodName, object[] parameters)
        {
            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(target, parameters);
        }
    }
}
