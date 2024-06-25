using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;

    private float xRotation = 0f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        
        // calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        
        // apply to our camera transform
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); 
        
        // rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
