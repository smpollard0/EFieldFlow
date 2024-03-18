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

    // Reference to the parent object (the object to be moved)
    private GameObject parentObject;

    // Store the initial mouse position
    private Vector3 initialMousePosition;

    void Start()
    {
        parentObject = GameObject.FindGameObjectWithTag("SelectedObject");
    }

    // Update is called once per frame
    void Update()
    {
        // if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
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
        if (Input.GetMouseButton(0))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed,
                                -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }

        // if the control button is held, zoom in and out (left or right control for left and right handed people c:)
        // also make sure the game isn't paused
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && !PauseMEnu.GameIsPaused)
        {
            transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
        }

        // Check for arrow dragging
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any colliders in the scene
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit collider belongs to an arrow
                if (hit.collider.CompareTag("XArrow") ||
                    hit.collider.CompareTag("YArrow") ||
                    hit.collider.CompareTag("ZArrow"))
                {
                    // Store the initial mouse position
                    initialMousePosition = Input.mousePosition;
                }
            }
        }

        // Check if mouse is being dragged
        if (Input.GetMouseButton(0))
        {
            // Calculate the mouse movement since the last frame
            Vector3 mouseDelta = (Input.mousePosition - initialMousePosition) * Time.deltaTime;

            // Move the parent object based on the arrow's tag
            MoveParentObject(mouseDelta);

            // Update the initial mouse position for the next frame
            initialMousePosition = Input.mousePosition;
        }
    }

    void MoveParentObject(Vector3 mouseDelta)
    {
        // Get the tag of the arrow
        string arrowTag = gameObject.tag;

        // Check which arrow we just clicked on
        switch (arrowTag)
        {
            case "XArrow":
                parentObject.transform.Translate(Vector3.right * Time.deltaTime * mouseDelta.x);
                break;
            case "YArrow":
                parentObject.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y);
                break;
            case "ZArrow":
                parentObject.transform.Translate(Vector3.forward * Time.deltaTime * mouseDelta.z);
                break;
            default:
                Debug.LogWarning("Unknown arrow tag: " + arrowTag);
                break;
        }
    }
}
