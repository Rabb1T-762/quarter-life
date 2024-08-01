using _Project.Tests.TestUtilities;
using NUnit.Framework;
using UnityEngine;

public class PlayerCameraControllerTests
{
    private GameObject _simulatedPlayerObject;
    private PlayerCameraController _playerCameraController;
    private Camera _camera;

    [SetUp]
    public void SetUp()
    {
        _simulatedPlayerObject = new GameObject();
        _playerCameraController = _simulatedPlayerObject.AddComponent<PlayerCameraController>();
        var cameraObject = new GameObject();
        var camera = cameraObject.AddComponent<Camera>();
        TestHelper.SetPrivateField(_playerCameraController, "playerCamera", camera);
        _camera = TestHelper.GetPrivateField<Camera>(_playerCameraController, "playerCamera");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_simulatedPlayerObject);
        Object.DestroyImmediate(_camera.gameObject);
    }

    [Test]
    public void ProcessLook_yInput_AppliesRotationToCamera()
    {
        Vector2 input = new Vector2(0, 1); // simulate mouse input for looking up
        _playerCameraController.ProcessLook(input);

        Assert.That(_camera.transform.localRotation.eulerAngles.x, Is.Not.EqualTo(0f));
    }

    [Test]
    public void ProcessLook_xInput_AppliesRotationToParentObject()
    {
        Vector2 input = new Vector2(1, 0); // simulate mouse input for looking right
        _playerCameraController.ProcessLook(input);

        Assert.That(_simulatedPlayerObject.transform.localRotation.eulerAngles.y, Is.Not.EqualTo(0f));
    }

    [Test]
    public void ProcessLook_LookUp_ClampsXRotation()
    {
        // Arrange
        var topClamp = TestHelper.GetPrivateField<float>(_playerCameraController, "topClamp");
        Vector2 input = new Vector2(0, 1000); // simulate mouse input for looking up

        // Act
        _playerCameraController.ProcessLook(input);
        var xRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_xRotation");

        // Assert
        Assert.That(xRotation, Is.GreaterThanOrEqualTo(topClamp));
    }

    [Test]
    public void ProcessLook_LookDown_ClampsXRotation()
    {
        // Arrange 
        var bottomClamp = TestHelper.GetPrivateField<float>(_playerCameraController, "bottomClamp");
        Vector2 input = new Vector2(0, -1000); // simulate mouse input for looking down

        // Act
        _playerCameraController.ProcessLook(input);
        var xRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_xRotation");

        // Assert
        Assert.That(xRotation, Is.LessThanOrEqualTo(bottomClamp));
    }

    [Test]
    public void ProcessLook_LookUp_DecrementsXRotation()
    {
        // Arrange 
        var initialXRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_xRotation");
        Vector2 input = new Vector2(0, 10); // simulate mouse input for looking down

        // Act
        _playerCameraController.ProcessLook(input);
        var actualXRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_xRotation");

        // Assert
        Assert.That(actualXRotation, Is.LessThan(initialXRotation));
    }

    [Test]
    public void ProcessLook_LookDown_IncrementsXRotation()
    {
        // Arrange 
        var initialXRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_xRotation");
        Vector2 input = new Vector2(0, -10); // simulate mouse input for looking down

        // Act
        _playerCameraController.ProcessLook(input);
        var actualXRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_xRotation");

        // Assert
        Assert.That(actualXRotation, Is.GreaterThan(initialXRotation));
    }

    [Test]
    public void ProcessLook_LookRight_IncrementsYRotation()
    {
        // Arrange
        Vector2 input = new Vector2(1, 0); // simulate mouse input for looking right
        var initialYRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_yRotation");

        // Act
        _playerCameraController.ProcessLook(input);
        var actualYRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_yRotation");

        // Assert
        Assert.That(actualYRotation, Is.GreaterThan(initialYRotation));
    }

    [Test]
    public void ProcessLook_LookLeft_DecrementsYRotation()
    {
        // Arrange
        Vector2 input = new Vector2(-1, 0); // simulate mouse input for looking left
        var initialYRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_yRotation");

        // Act
        _playerCameraController.ProcessLook(input);
        var actualYRotation = TestHelper.GetPrivateField<float>(_playerCameraController, "_yRotation");

        // Assert
        Assert.That(actualYRotation, Is.LessThan(initialYRotation));
    }
}