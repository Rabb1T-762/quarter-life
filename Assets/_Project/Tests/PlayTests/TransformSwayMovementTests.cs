using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using _Project.Scripts;
using _Project.Tests.TestUtilities;
using System.Reflection;

public class TransformSwayMovementTests
{
    private GameObject _swayMovementObject;
    private TransformSwayMovement _swayMovement;
    private Transform _transform;

    [SetUp]
    public void SetUp()
    {
        _swayMovementObject = new GameObject();
        _swayMovement = _swayMovementObject.AddComponent<TransformSwayMovement>();
        _transform = _swayMovementObject.transform;

        // Use reflection to set private serialized fields
        TestHelper.SetPrivateField(_swayMovement, "transform", _transform);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_swayMovementObject);
    }

    [Test]
    public void CalculateSway_UpdatesSwayPosition()
    {
        // Arrange
        Vector3 initialPosition = _transform.localPosition;
        float initialSwayTime = TestHelper.GetPrivateField<float>(_swayMovement, "_swayTime");

        // Act
        var calculateSwayMethod = TestHelper.GetPrivateMethod("CalculateSway", _swayMovement);
        calculateSwayMethod.Invoke(_swayMovement, new object[] {});

        // Assert
        Vector3 swayPosition = TestHelper.GetPrivateField<Vector3>(_swayMovement, "_swayPosition");
        Assert.That(swayPosition, Is.Not.EqualTo(Vector3.zero));

        Vector3 newPosition = _transform.localPosition;
        Assert.That(newPosition, Is.Not.EqualTo(initialPosition));

        float newSwayTime = TestHelper.GetPrivateField<float>(_swayMovement, "_swayTime");
        Assert.That(newSwayTime, Is.GreaterThan(initialSwayTime));
    }

    [Test]
    public void SwayCurve_ReturnsCorrectVector()
    {
        // Arrange
        float time = 1f;
        float curveXAmplitude = 1f;
        float curveXFrequency = 1f;
        float curveYAmplitude = 1f;
        float curveYFrequency = 1f;
        float apparentRotation = Mathf.PI;

        float expectedX = curveXAmplitude * Mathf.Sin(curveXFrequency * time);
        float expectedY = curveYAmplitude * Mathf.Sin(curveYFrequency * time + apparentRotation);

        var swayMovementFunction = TestHelper.GetPrivateMethod("SwayCurve", _swayMovement);

        // Act
        var result = (Vector3)swayMovementFunction.Invoke(_swayMovement,
            new object[]
                { time, curveXAmplitude, curveXFrequency, curveYAmplitude, curveYFrequency, apparentRotation });


        // Assert
        Assert.That(result, Is.EqualTo(new Vector3(expectedX, expectedY)));
    }

    [Test]
    public void SwayCurve_HandlesZeroAmplitude()
    {
        // Arrange
        float time = 1f;
        float curveXAmplitude = 0f;
        float curveXFrequency = 1f;
        float curveYAmplitude = 0f;
        float curveYFrequency = 1f;
        float apparentRotation = Mathf.PI;

        var swayMovementFunction = TestHelper.GetPrivateMethod("SwayCurve", _swayMovement);

        // Act
        var result = (Vector3)swayMovementFunction.Invoke(_swayMovement,
            new object[]
                { time, curveXAmplitude, curveXFrequency, curveYAmplitude, curveYFrequency, apparentRotation });

        // Assert
        Assert.That(result, Is.EqualTo(Vector3.zero));
    }
}