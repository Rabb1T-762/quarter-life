using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;
    [SerializeField] private float topClamp = -89f;
    [SerializeField] private float bottomClamp = 89f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        
        // calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        
        // clamp the rotation of the camera
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        
        // calculate the rotation for the player to look left and right
        yRotation += (mouseX * Time.deltaTime) * xSensitivity;
        
        // apply rotations to our transform
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f); 
    }

}
