using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    // Prefabs for x, y, z manipulation arrows.
    public GameObject arrowPrefabX;
    public GameObject arrowPrefabY;
    public GameObject arrowPrefabZ;

    public float mouseSensitivity = 1000f;
    float xRotation = 0f;
    float yRotation = 0f;

    public float dragSpeed = 6f;
    public float zoomSpeed = 30f;

    private GameObject selectedObject; // variable for selected gameobject
    private GameObject arrowsParent; // parent object for arrows

    private Camera mainCamera; // Reference to the main camera
    private Material outlineMaterial; // Currently unused material for attempting to highlight a selected object

    private Vector3 initialMousePosition;

    // Track if interacting with an arrow
    private bool interactingWithArrow = false;

    void Awake() {
        mainCamera = Camera.main;
        outlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");

        if (outlineMaterial == null) {
            Debug.LogError("Outline material not found!");
        }
    }

    void Update() {
        // Handle camera movement
        HandleCameraMovement();

        // Select and manipulate objects
        HandleObjectInteraction();
    }

    void HandleCameraMovement() {
        // Only allow camera movement if not interacting with an arrow
        if (!interactingWithArrow)
        {
            // Camera rotation
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                yRotation += mouseX;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            }

            // Camera dragging
            if (Input.GetMouseButton(0))
            {
                transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed,
                                    -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
            }

            // Camera zoom
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)))
            {
                transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
            }
        }
    }

    void HandleObjectInteraction() {
        if (Input.GetMouseButtonDown(0) && !PauseMEnu.GameIsPaused)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Sphere"))
                {
                    GameObject newSelection = hit.collider.gameObject;

                    if (selectedObject != null && selectedObject != newSelection)
                    {
                        DestroyArrows();
                    }

                    selectedObject = newSelection;
                    InstantiateArrows(selectedObject.transform.position);
                }
                else if (hit.collider.CompareTag("Xarrow") || hit.collider.CompareTag("Yarrow") || hit.collider.CompareTag("Zarrow"))
                {
                    // If an arrow is clicked, move the parent object
                    MoveParentObject(hit.collider.tag);
                    Debug.Log("Arrow clicked");
                    interactingWithArrow = true; // Set interacting with arrow to true
                }
                else
                {
                    DeselectObject();
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // Reset interacting with arrow when mouse button is released
        {
            interactingWithArrow = false;
        }

        if (Input.GetKeyDown("space") && !PauseMEnu.GameIsPaused)
        {
            DeselectObject();
        }
    }

    void InstantiateArrows(Vector3 position) {
        float offset = 1.0f;

        arrowPrefabX.tag = "Xarrow";
        arrowPrefabY.tag = "Yarrow";
        arrowPrefabZ.tag = "Zarrow";

        GameObject arrowX = Instantiate(arrowPrefabX, position + Vector3.right * offset, Quaternion.identity);
        arrowX.transform.rotation = Quaternion.Euler(0, 90, 0);

        GameObject arrowY = Instantiate(arrowPrefabY, position + Vector3.up * offset, Quaternion.identity);
        arrowY.transform.rotation = Quaternion.Euler(-90, 0, 0);

        GameObject arrowZ = Instantiate(arrowPrefabZ, position + Vector3.forward * offset, Quaternion.identity);

        arrowsParent = new GameObject("Arrows");
        arrowX.transform.parent = arrowsParent.transform;
        arrowY.transform.parent = arrowsParent.transform;
        arrowZ.transform.parent = arrowsParent.transform;
        arrowsParent.transform.parent = selectedObject.transform;
    }

    void DeselectObject() {
        if (selectedObject != null)
        {
            DestroyArrows();
            selectedObject = null;
        }
    }

    void DestroyArrows() {
        if (arrowsParent != null)
        {
            Destroy(arrowsParent);
        }
    }

    void MoveParentObject(string arrowTag) {
        float movementSpeed = 5f; // Adjust as needed

        switch (arrowTag)
        {
            case "Xarrow":
                selectedObject.transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
                break;
            case "Yarrow":
                selectedObject.transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
                break;
            case "Zarrow":
                selectedObject.transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
                break;
            default:
                Debug.LogWarning("Unknown arrow tag: " + arrowTag);
                break;
        }
    }
}
