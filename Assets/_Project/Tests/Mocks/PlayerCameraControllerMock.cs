using System;
using UnityEngine;

public class PlayerCameraControllerMock : PlayerCameraController
{
    public new Camera playerCamera;

    public void Start()
    {
        var cameraGameObject = new GameObject("MockCamera");
        playerCamera = cameraGameObject.AddComponent<Camera>();
    }

    public override void ProcessLook(Vector2 input)
    {
    }
}