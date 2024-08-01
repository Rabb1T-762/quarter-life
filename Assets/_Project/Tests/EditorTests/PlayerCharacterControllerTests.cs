using _Project.Scripts;
using _Project.Tests.TestUtilities;
using NUnit.Framework;
using UnityEngine;

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

            TestHelper.SetPrivateField(_playerController, "groundCheck", _groundCheck);
            TestHelper.SetPrivateField(_playerController, "_controller", _characterController);
            TestHelper.SetPrivateField(_playerController, "_inputManager", _inputManager);
            TestHelper.SetPrivateField(_playerController, "_cameraController", _cameraController);
            TestHelper.SetPrivateField(_playerController, "mainWeapon", _weapon);

            TestHelper.SetPrivateField(_playerController, "playerMaxGroundedSpeed", 10f);
            TestHelper.SetPrivateField(_playerController, "playerMaxAirborneSpeed", 15f);
            TestHelper.SetPrivateField(_playerController, "playerWalkSpeedMultiplier", 0.5f);
            TestHelper.SetPrivateField(_playerController, "playerCrouchSpeedMultiplier", 0.4f);
            TestHelper.SetPrivateField(_playerController, "playerAcceleration", 100f);
            TestHelper.SetPrivateField(_playerController, "transformCrouchHeight", 1f);
            TestHelper.SetPrivateField(_playerController, "transformStandingHeight", 2f);
            TestHelper.SetPrivateField(_playerController, "heightTransitionSpeed", 10f);
            TestHelper.SetPrivateField(_playerController, "groundDistance", 0.2f);
            TestHelper.SetPrivateField(_playerController, "groundMask", new LayerMask());
            TestHelper.SetPrivateField(_playerController, "gravity", -9.8f);
            TestHelper.SetPrivateField(_playerController, "jumpHeight", 2f);
            TestHelper.SetPrivateField(_playerController, "jumpForceMultiplier", -2.0f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_player);
        }
    }

}
