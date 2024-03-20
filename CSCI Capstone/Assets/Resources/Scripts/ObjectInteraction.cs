using System.Collections;
using System.Collections.Generic;
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

    private bool isObjectDragged = false; // Flag to track if an object is being dragged


    void Awake() {
        mainCamera = Camera.main;
        outlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");

        if (outlineMaterial == null) {
            Debug.LogError("Outline material not found!");
        }
    }

    void Update() {
        if (!isObjectDragged){
            // Handle camera movement
            HandleCameraMovement();
        }

        // Select and manipulate objects
        HandleObjectInteraction();
    }

    void HandleCameraMovement() {
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
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && !PauseMEnu.GameIsPaused)
        {
            transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
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
                        DeselectObject();
                    }

                    isObjectDragged = true;
                    selectedObject = newSelection;
                    InstantiateArrows(selectedObject.transform.position);
                }
            }
        } else if (Input.GetMouseButtonUp(0)) {
            isObjectDragged = false;
        }

        if (Input.GetKeyDown("space") && !PauseMEnu.GameIsPaused)
        {
            DeselectObject();
        }
    }

    void InstantiateArrows(Vector3 position) {
         // Check if arrows already exist on the selected object
        if (selectedObject.transform.Find("ArrowX") == null) {
            float offset = 1.0f;

            arrowPrefabX.tag = "Xarrow";
            arrowPrefabY.tag = "Yarrow";
            arrowPrefabZ.tag = "Zarrow";

            GameObject arrowX = Instantiate(arrowPrefabX, position + Vector3.right * offset, Quaternion.identity);
            arrowX.name = "ArrowX";
            arrowX.transform.rotation = Quaternion.Euler(0, 90, 0);
            arrowX.transform.parent = selectedObject.transform;

            GameObject arrowY = Instantiate(arrowPrefabY, position + Vector3.up * offset, Quaternion.identity);
            arrowY.name = "ArrowY";
            arrowY.transform.rotation = Quaternion.Euler(-90, 0, 0);
            arrowY.transform.parent = selectedObject.transform;

            GameObject arrowZ = Instantiate(arrowPrefabZ, position + Vector3.forward * offset, Quaternion.identity);
            arrowZ.name = "ArrowZ";
            arrowZ.transform.parent = selectedObject.transform;
            arrowsParent = selectedObject;
        }
    }

    void DestroyArrows() {
        if (selectedObject != null)
        {
            // Destroy individual arrows
            foreach (Transform child in selectedObject.transform)
            {
                if (child.CompareTag("Xarrow") || child.CompareTag("Yarrow") || child.CompareTag("Zarrow"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }


    void DeselectObject() {
        if (selectedObject != null)
        {
            DestroyArrows();
            selectedObject = null;
        }
    }

}
