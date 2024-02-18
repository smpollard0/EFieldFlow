using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    float xRotation = 0f;
    float yRotation = 0f;

    public float dragSpeed = 6f;
    public float zoomSpeed = 30f;

    // Update is called once per frame
    void Update() {
        // GetMouseButton(0) = LMB
        // GetMouseButton(1) = RMB
        // if the right mouse button is held down
        if (Input.GetMouseButton(1)){

            // capture mouse movement independent of framerate by multiplying the mouse movement by Time.deltaTime
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // when the user moves their mouse to the left, the camera rotates right and vice versa
            xRotation -= mouseY;
            yRotation += mouseX;
            // clamp the rotation so the user doesn't just keep rotating upward or downward
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            // use the components of the turn vector to rotate the camera
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f); 
        }

        // drag the camera around the scene using LMB
        if (Input.GetMouseButton(0)){
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, 
                                -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }

        // if the control button is held, zoom in and out (left or right control for left and right handed people c:)
        // also make sure the game isn't paused
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && !PauseMEnu.GameIsPaused){
            transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
        }
        

    }
}
