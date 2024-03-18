using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Reference to the parent object (the object to be moved)
    private GameObject parentObject;

    // Store the initial mouse position
    private Vector3 initialMousePosition;

    void Start()
    {
        // Find the parent object by moving up the hierarchy
        parentObject = transform.parent.gameObject;
    }

    void OnMouseDown()
    {
        // Store the initial mouse position
        initialMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        // Calculate the mouse movement since the last frame
        Vector3 mouseDelta = (Input.mousePosition - initialMousePosition) * Time.deltaTime;

        // Move the parent object based on the arrow's tag
        MoveParentObject(mouseDelta);

        // Update the initial mouse position for the next frame
        initialMousePosition = Input.mousePosition;
    }

    void MoveParentObject(Vector3 mouseDelta)
    {

        // Get the tag of the arrow
        string arrowTag = gameObject.tag;

        // Check which arrow we just clicked on
        switch (arrowTag)
        {
            case "Xarrow":
                parentObject.transform.Translate(Vector3.right * Time.deltaTime * mouseDelta.x);
                break;
            case "Yarrow":
                parentObject.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y);
                break;
            case "Zarrow":
                parentObject.transform.Translate(Vector3.forward * Time.deltaTime * mouseDelta.z);
                break;
            default:
                Debug.LogWarning("Unknown arrow tag: " + arrowTag);
                break;
        }
    }
}
