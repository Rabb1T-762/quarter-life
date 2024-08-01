using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;
    [SerializeField] private float topClamp = -89f;
    [SerializeField] private float bottomClamp = 89f;

    private float _xRotation;
    private float _yRotation;

    public virtual void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // calculate camera rotation for looking up and down
        // it's x because rotation happens around the axis 
        // Think of a gear rotating around a turning shaft.
        _xRotation -= (mouseY * Time.deltaTime) * ySensitivity;

        // clamp the rotation of the camera
        _xRotation = Mathf.Clamp(_xRotation, topClamp, bottomClamp);

        // calculate the rotation for the player to look left and right
        _yRotation += (mouseX * Time.deltaTime) * xSensitivity;

        // apply rotations to our transform
        playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}