using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private GameObject parentObject;
    private Vector3 initialMousePosition;
    private Vector3 initialObjectPosition;
    public bool isDragging = false;

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

        // Determine movement direction based on arrow axis
        Vector3 movementDirection = Vector3.zero;
        if (gameObject.CompareTag("Xarrow")){
            movementDirection = Vector3.right;
            movementAmount = Vector3.Dot(mouseDelta, Camera.main.transform.right);
        }
            
        else if (gameObject.CompareTag("Yarrow")){
            movementDirection = Vector3.up;
            movementAmount = Vector3.Dot(mouseDelta, Camera.main.transform.up);
        }
            
        else if (gameObject.CompareTag("Zarrow")){
            movementDirection = Vector3.forward;
            movementAmount = Vector3.Dot(mouseDelta, Camera.main.transform.right);
        }        

        // Translate parent object position along the specified axis
        parentObject.transform.position = initialObjectPosition + (movementDirection * movementAmount * 0.01f);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
