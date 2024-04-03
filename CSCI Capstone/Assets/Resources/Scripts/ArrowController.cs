using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private GameObject parentObject;
    private Vector3 initialMousePosition;
    private Vector3 initialObjectPosition;
    public static bool isDragging = false;

    void Start()
    {
        parentObject = transform.parent.gameObject;
    }

    void OnMouseDown()
    {
        isDragging = true;
        initialMousePosition = Input.mousePosition;
        initialObjectPosition = parentObject.transform.position;
    }

    void OnMouseDrag()
    {
        if (!isDragging)
        return;

        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - initialMousePosition;
        float movementAmount = 0f;

        // Convert screen movement to world space movement
        Vector3 worldMovement = Camera.main.transform.TransformDirection(mouseDelta);

        // Extract movement along the arrow's axis
        Vector3 movementDirection = Vector3.zero;
        if (gameObject.CompareTag("Xarrow"))
        {
            movementAmount = worldMovement.x;
            movementDirection = Vector3.right;
        }
        else if (gameObject.CompareTag("Yarrow"))
        {
            movementAmount = worldMovement.y;
            movementDirection = Vector3.up;
        }
        else if (gameObject.CompareTag("Zarrow"))
        {
            movementAmount = worldMovement.z;
            movementDirection = Vector3.forward;
        }

        // Translate parent object position along the specified axis
        parentObject.transform.position = initialObjectPosition + (movementDirection * movementAmount * 0.01f);

    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
