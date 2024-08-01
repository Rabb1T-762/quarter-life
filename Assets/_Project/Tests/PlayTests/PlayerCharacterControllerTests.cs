using System.Collections;
using _Project.Scripts;
using _Project.Tests.Mocks;
using _Project.Tests.TestUtilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerCharacterControllerTests : MonoBehaviour
{
    private GameObject _playerGameObject;
    private PlayerCharacterController _playerCharacterController;
    private CharacterController _controller;
    private InputManager _inputManager;
    private PlayerCameraControllerMock _cameraController;
    private Transform _groundCheck;
    private LayerMask _layerMask;
    private Weapon _weapon;

    [SetUp]
    public void Setup()
    {
        _playerGameObject = new GameObject();

        _controller = _playerGameObject.AddComponent<CharacterController>();
        _inputManager = _playerGameObject.AddComponent<InputManagerMock>();
        _cameraController = _playerGameObject.AddComponent<PlayerCameraControllerMock>();
        _playerCharacterController = _playerGameObject.AddComponent<PlayerCharacterController>();

        // Set the mock PlayerCameraController in the PlayerCharacterController
        TestHelper.SetPrivateField(_playerCharacterController, "_cameraController", _cameraController);
        TestHelper.SetPrivateField(_playerCharacterController, "_inputManager", _inputManager);
        TestHelper.SetPrivateField(_playerCharacterController, "_controller", _controller);

        _groundCheck = new GameObject().transform;
        TestHelper.SetPrivateField(_playerCharacterController, "groundCheck", _groundCheck);

        _layerMask = LayerMask.NameToLayer("Default");
        TestHelper.SetPrivateField(_playerCharacterController, "groundMask", _layerMask);

        var weaponObject = new GameObject();
        _weapon = weaponObject.AddComponent<Weapon>();
        TestHelper.SetPrivateField(_playerCharacterController, "mainWeapon", _weapon);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_playerGameObject);
    }

    [UnityTest]
    public IEnumerator Jump_CausesTheParentTransformToJumpUp()
    {
        // Arrange
        var initialPosition = _playerGameObject.transform.position;
        var jumpMethod = TestHelper.GetPrivateMethod("Jump", _playerCharacterController);

        // Act
        jumpMethod.Invoke(_playerCharacterController, new object[] { });
        yield return null;

        var actual = _playerGameObject.transform.position;

        // Assert
        Assert.That(actual.y, Is.GreaterThan(initialPosition.y));
    }

    [UnityTest]
    public IEnumerator Jump_AddsYVelocityToParentObject()
    {
        // Arrange
        var initialVelocity = TestHelper.GetPrivateField<Vector3>(_playerCharacterController, "_playerVelocity");
        var jumpMethod = TestHelper.GetPrivateMethod("Jump", _playerCharacterController);

        // Act
        jumpMethod.Invoke(_playerCharacterController, new object[] { });
        yield return null;
        var actual = TestHelper.GetPrivateField<Vector3>(_playerCharacterController, "_playerVelocity");

        // Assert
        Assert.That(actual.y, Is.GreaterThan(initialVelocity.y));
    }


    [UnityTest]
    public IEnumerator Walk_MultipliesMovementMultiplierByWalkSpeed()
    {
        // Arrange
        var walkSpeedMultiplier =
            TestHelper.GetPrivateField<float>(_playerCharacterController, "playerWalkSpeedMultiplier");
        var initialMovementMultiplier =
            TestHelper.GetPrivateField<float>(_playerCharacterController, "_movementMultiplier");
        var walkMethod = TestHelper.GetPrivateMethod("Walk", _playerCharacterController);
        var expected = initialMovementMultiplier * walkSpeedMultiplier;

        // Act
        walkMethod.Invoke(_playerCharacterController, new object[] { });
        var actualMultiplier = TestHelper.GetPrivateField<float>(_playerCharacterController, "_movementMultiplier");

        // Assert
        Assert.That(actualMultiplier, Is.EqualTo(expected));

        yield return null;
    }


    [UnityTest]
    public IEnumerator Crouch_MultipliesMovementMultiplierByCrouchSpeed()
    {
        // Arrange
        var crouchSpeedMultiplier =
            TestHelper.GetPrivateField<float>(_playerCharacterController, "playerCrouchSpeedMultiplier");
        var initialMovementMultiplier =
            TestHelper.GetPrivateField<float>(_playerCharacterController, "_movementMultiplier");
        var crouchMethod = TestHelper.GetPrivateMethod("Crouch", _playerCharacterController);
        var expected = initialMovementMultiplier * crouchSpeedMultiplier;

        // Act
        crouchMethod.Invoke(_playerCharacterController, new object[] { });

        var actualMultiplier = TestHelper.GetPrivateField<float>(_playerCharacterController, "_movementMultiplier");

        // Assert
        Assert.That(actualMultiplier, Is.EqualTo(expected));

        yield return null;
    }


    [UnityTest]
    public IEnumerator Crouch_SetsTransformHeightToCrouchHeight()
    {
        // Arrange
        var crouchHeight = TestHelper.GetPrivateField<float>(_playerCharacterController, "transformCrouchHeight");
        var crouchMethod = TestHelper.GetPrivateMethod("Crouch", _playerCharacterController);

        // Act
        crouchMethod.Invoke(_playerCharacterController, new object[] { });

        var actualHeight = TestHelper.GetPrivateField<float>(_playerCharacterController, "_targetTransformHeight");

        // Assert
        Assert.That(actualHeight, Is.EqualTo(crouchHeight));
        yield return null;
    }


    [UnityTest]
    public IEnumerator UpdateCharacterHeight_AdjustsControllerHeight()
    {
        // Arrange
        var initialHeight = _controller.height;
        var targetHeight = initialHeight + 1.0f; // Some target different from initial
        TestHelper.SetPrivateField(_playerCharacterController, "_targetTransformHeight", targetHeight);
        var heightTransitionSpeed =
            TestHelper.GetPrivateField<float>(_playerCharacterController, "heightTransitionSpeed");
        var handleCharacterHeightMethod =
            TestHelper.GetPrivateMethod("UpdateCharacterHeight", _playerCharacterController);

        // Act
        handleCharacterHeightMethod.Invoke(_playerCharacterController, new object[] { });
        yield return new WaitForSeconds(Time.deltaTime); // Wait for one frame

        var actualHeight = _controller.height;

        // Calculate expected height after one frame
        var expectedHeight = Mathf.Lerp(initialHeight, targetHeight, Time.deltaTime * heightTransitionSpeed);

        // Assert
        Assert.That(actualHeight, Is.EqualTo(expectedHeight).Within(0.1f));
    }


    [UnityTest]
    public IEnumerator UpdateGroundCheckPosition_SetsGroundCheckPositionToBottomOfParentObject()
    {
        // Arrange
        var initialHeight = _controller.height;
        var targetHeight = initialHeight + 1.0f; // Some target different from initial
        TestHelper.SetPrivateField(_playerCharacterController, "_targetTransformHeight", targetHeight);
        var updateGroundCheckPositionMethod =
            TestHelper.GetPrivateMethod("UpdateGroundCheckPosition", _playerCharacterController);

        // Act
        updateGroundCheckPositionMethod.Invoke(_playerCharacterController, new object[] { });
        yield return new WaitForSeconds(Time.deltaTime); // Wait for one frame

        // Calculate expected ground check position
        Vector3 expectedPlayerObjectBottom = _controller.center - new Vector3(0, _controller.height / 2, 0);
        var groundCheckTransform = TestHelper.GetPrivateField<Transform>(_playerCharacterController, "groundCheck");
        var actualGroundCheckPosition = groundCheckTransform.position;

        // Assert
        Assert.That(actualGroundCheckPosition, Is.EqualTo(expectedPlayerObjectBottom));
    }
}