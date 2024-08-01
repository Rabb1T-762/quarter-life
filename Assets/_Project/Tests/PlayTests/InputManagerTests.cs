using System.Collections;
using _Project.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class PlayerInputHandlerTests
{
    private GameObject _playerGameObject;
    private InputManager _playerInputHandler;
    private InputTestFixture _inputTestFixture;
    private Keyboard _keyboard;
    private Mouse _mouse;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject for the player
        _playerGameObject = new GameObject("Player");
        _playerInputHandler = _playerGameObject.AddComponent<InputManager>();
        _inputTestFixture = new InputTestFixture();

        _keyboard = InputSystem.AddDevice<Keyboard>();
        _mouse = InputSystem.AddDevice<Mouse>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_playerGameObject);

        InputSystem.RemoveDevice(_keyboard);
        InputSystem.RemoveDevice(_mouse);
        _inputTestFixture.TearDown();
    }

    [UnityTest]
    public IEnumerator GetMoveInput_ReturnsCorrectVector_WhenForwardKeyPressedOnKeyboard()
    {
        // Arrange 
        Vector3 expected = new Vector3(0, 0, 1);

        // Act 
        _inputTestFixture.Press(_keyboard.wKey);
        yield return null; // Wait for the next frame to allow input to update
        Vector3 actual = _playerInputHandler.GetMoveInput();

        _inputTestFixture.Release(_keyboard.wKey);

        // Assert
        Assert.That(expected == actual);
    }

    [UnityTest]
    public IEnumerator GetLookInput_ReturnsCorrectVector_WhenMouseMoved()
    {
        // Arrange
        Vector2 expected = new Vector2(10f, 5f); // Example expected mouse movement input

        // Simulate mouse movement
        _inputTestFixture.Move(_mouse.position, new Vector2(10f, 5f));

        // Act
        Vector2 actual = _playerInputHandler.GetLookInput();

        // Assert
        Assert.AreEqual(expected, actual);

        // Cleanup
        _inputTestFixture.Move(_mouse.position, new Vector2(0, 0));
        yield return null;
    }

    [UnityTest]
    public IEnumerator GetLookInput_ReturnsZeroVector_WhenMouseNotMoved()
    {
        // Arrange
        _inputTestFixture.Set(_mouse.position, new Vector2(0, 0));
        Vector2 expected = Vector2.zero;

        // Waiting one frame for mouse.delta to reset to zero
        // in case of mouse not at the above vector
        yield return null;

        // Act
        Vector2 actual = _playerInputHandler.GetLookInput();

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [UnityTest]
    public IEnumerator GetLookInput_ReturnsPositiveY_WhenMouseMovedUp()
    {
        // Arrange
        _inputTestFixture.Move(_mouse.position, new Vector2(0, 100));

        // Act
        Vector2 actual = _playerInputHandler.GetLookInput();

        // Assert
        Assert.Greater(actual.y, 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GetLookInput_ReturnsNegativeY_WhenMouseMovedDown()
    {
        // Arrange
        _inputTestFixture.Move(_mouse.position, new Vector2(0, -100));

        // Act
        Vector2 actual = _playerInputHandler.GetLookInput();

        // Assert
        Assert.Less(actual.y, 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GetLookInput_ReturnsNegativeX_WhenMouseMovedLeft()
    {
        // Arrange
        _inputTestFixture.Move(_mouse.position, new Vector2(-100, 0));

        // Act
        Vector2 actual = _playerInputHandler.GetLookInput();

        // Assert
        Assert.Less(actual.x, 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GetLookInput_ReturnsPositiveX_WhenMouseMovedRight()
    {
        // Arrange
        _inputTestFixture.Move(_mouse.position, new Vector2(100, 0));

        // Act
        Vector2 actual = _playerInputHandler.GetLookInput();

        // Assert
        Assert.Greater(actual.x, 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GetMoveInput_ReturnsCorrectVector_WhenBackwardsKeyPressedOnKeyboard()
    {
        // Arrange 
        Vector3 expected = new Vector3(0, 0, -1);

        // Act 
        _inputTestFixture.Press(_keyboard.sKey);
        yield return null; // Wait for the next frame to allow input to update
        Vector3 actual = _playerInputHandler.GetMoveInput();

        _inputTestFixture.Release(_keyboard.sKey);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [UnityTest]
    public IEnumerator GetMoveInput_ReturnsCorrectVector_WhenLeftKeyPressedOnKeyboard()
    {
        // Arrange 
        Vector3 expected = new Vector3(-1, 0, 0);

        // Act 
        _inputTestFixture.Press(_keyboard.aKey);
        yield return null; // Wait for the next frame to allow input to update
        Vector3 actual = _playerInputHandler.GetMoveInput();

        _inputTestFixture.Release(_keyboard.aKey);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [UnityTest]
    public IEnumerator GetMoveInput_ReturnsCorrectVector_WhenRightKeyPressedOnKeyboard()
    {
        // Arrange 
        Vector3 expected = new Vector3(1, 0, 0);

        // Act 
        _inputTestFixture.Press(_keyboard.dKey);
        yield return null; // Wait for the next frame to allow input to update
        Vector3 actual = _playerInputHandler.GetMoveInput();

        _inputTestFixture.Release(_keyboard.dKey);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [UnityTest]
    public IEnumerator GetMoveInput_ReturnsCorrectVector_WhenMovingDiagonally()
    {
        // Arrange
        Vector3 expected = new Vector3(0.71f, 0f, 0.71f);

        // Act
        _inputTestFixture.Press(_keyboard.wKey);
        _inputTestFixture.Press(_keyboard.dKey);
        yield return null; // Wait for the next frame to allow input to update

        Vector3 actual = _playerInputHandler.GetMoveInput();

        _inputTestFixture.Release(_keyboard.wKey);
        _inputTestFixture.Release(_keyboard.dKey);

        // Assert
        Debug.Log("Actual: " + actual + "  Expected: " + expected);
        Assert.AreEqual(expected.x, actual.x, 0.01f);
        Assert.AreEqual(expected.y, actual.y, 0.01f);
        Assert.AreEqual(expected.z, actual.z, 0.01f);
    }

    [UnityTest]
    public IEnumerator GetJumpInput_ReturnsTrue_WhenJumpKeyPressedOnKeyboard()
    {
        // Arrange 
        // Act 
        _inputTestFixture.Press(_keyboard.spaceKey);
        yield return null; // Wait for the next frame to allow input to update
        bool actual = _playerInputHandler.GetJumpInput();

        _inputTestFixture.Release(_keyboard.spaceKey);

        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }

    [UnityTest]
    public IEnumerator GetJumpInput_ReturnsFalse_WhenJumpKeyNotPressed()
    {
        // Arrange 
        // Act 
        _inputTestFixture.Release(_keyboard.spaceKey);
        yield return null; // Wait for the next frame to allow input to update
        bool actual = _playerInputHandler.GetJumpInput();

        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }

    [UnityTest]
    public IEnumerator GetWalkInputHeld_ReturnsTrue_WhenWalkKeyPressedOnKeyboard()
    {
        // Arrange 

        // Act 
        _inputTestFixture.Press(_keyboard.shiftKey); // Assuming shift is the walk key
        yield return null; // Wait for the next frame to allow input to update
        bool actual = _playerInputHandler.GetWalkInputHeld();

        _inputTestFixture.Release(_keyboard.shiftKey);

        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }

    [UnityTest]
    public IEnumerator GetWalkInputHeld_ReturnsFalse_WhenWalkKeyNotPressed()
    {
        // Arrange 

        // Act 
        _inputTestFixture.Release(_keyboard.shiftKey); // Ensure the walk key is not pressed
        yield return null; // Wait for the next frame to allow input to update
        bool actual = _playerInputHandler.GetWalkInputHeld();

        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }

    [UnityTest]
    public IEnumerator GetCrouchInputHeld_ReturnsTrue_WhenCrouchKeyPressedOnKeyboard()
    {
        // Arrange

        // Act
        _inputTestFixture.Press(_keyboard.ctrlKey); // Assuming ctrl is the crouch key
        yield return null; // Wait for the next frame to allow input to update
        bool actual = _playerInputHandler.GetCrouchInputHeld();

        _inputTestFixture.Release(_keyboard.ctrlKey);

        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }

    [UnityTest]
    public IEnumerator GetCrouchInputHeld_ReturnsFalse_WhenCrouchKeyNotPressed()
    {
        // Arrange

        // Act
        _inputTestFixture.Release(_keyboard.ctrlKey); // Ensure the crouch key is not pressed
        yield return null; // Wait for the next frame to allow input to update
        bool actual = _playerInputHandler.GetCrouchInputHeld();

        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }

    [UnityTest]
    public IEnumerator GetTriggerInputPressed_ReturnsTrue_WhenShootKeyPressedOnKeyboard()
    {
        // Arrange

        // Act
        _inputTestFixture.Press(_mouse.leftButton); // Assuming space is the shoot key
        yield return null; // Wait for the next frame to allow input to update
        bool actual = _playerInputHandler.GetTriggerInputPressed();

        _inputTestFixture.Release(_mouse.leftButton);

        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }

    [UnityTest]
    public IEnumerator GetTriggerInputPressed_ReturnsFalse_WhenShootKeyNotPressed()
    {
        // Arrange
        // Ensure the shoot key is not pressed
        _inputTestFixture.Release(_mouse.leftButton);
        yield return null; // Wait for the next frame to allow input to update

        // Act
        bool actual = _playerInputHandler.GetTriggerInputPressed();

        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }
}