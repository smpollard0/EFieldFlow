using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    // Update is called once per frame
    void Update()
    {
        // if the right mouse button is held down
        if (Input.GetMouseButton(1)){
            // what the fuck does Time.deltaTime do?
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            // use the components of the turn vector to rotate the camera
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
