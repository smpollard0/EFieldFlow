using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjects : MonoBehaviour
{
    // Prefabs for x,y,z manipulation arrows.
    public GameObject arrowPrefabX;
    public GameObject arrowPrefabY;
    public GameObject arrowPrefabZ;

    private Camera mainCamera; // Reference to the main camera
    private GameObject selectedObject; // variable for selected gameobject
    private Material outlineMaterial;
    private Material originalMaterial;


    void Awake(){
        // reference the camera (this feels dumb considering this script is attached to the camera)
        mainCamera = Camera.main;

        outlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");

        if (outlineMaterial == null)
        {
            Debug.LogError("Outline material not found!");
        }

    }

    // Update is called once per frame
    void Update() {
        // select an object
        if (Input.GetMouseButtonDown(0) && !PauseMEnu.GameIsPaused) {
            // Cast a ray from the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any colliders in the scene
            if (Physics.Raycast(ray, out hit)) {
                // Check if the hit collider belongs to a sphere
                if (hit.collider.CompareTag("Sphere")) {
                    // Handle selection behavior
                    GameObject newSelection = hit.collider.gameObject;
                    Debug.Log("Selected sphere: " + newSelection.name);

                    // If there was a previous selection, destroy its arrows
                    if (selectedObject != null && selectedObject != newSelection) {
                        DestroyArrows();
                    }

                    // Set the currently selected object
                    selectedObject = newSelection;

                    // This will create the arrow prefabs at the object
                    InstantiateArrows(selectedObject.transform.position);
                } else {
                    // If the raycast doesn't hit a sphere, deselect the current object and destroy its arrows
                    DeselectObject();
                    Debug.Log("Arrows destroyed");
                }
            }
        }

        // Deselect objects when the spacebar is pressed
        if (Input.GetKeyDown("space") && !PauseMEnu.GameIsPaused){
            DeselectObject();
            Debug.Log("Arrows destroyed");
        }
    }

    void DeselectObject() {
        // If there is a selected object
        if (selectedObject != null) {
            // Destroy the arrows on the selected object
            DestroyArrows();

            // Deselect the object
            selectedObject = null;
        }
    }


    void InstantiateArrows(Vector3 position) {
        // Offset values for arrow positions
        float offset = 1.0f;

        // Instantiate arrow objects in the x, y, and z directions
        GameObject arrowX = Instantiate(arrowPrefabX, position + Vector3.right * offset, Quaternion.identity);
        arrowX.transform.rotation = Quaternion.Euler(0, 90, 0); // Rotate arrowX 90 degrees around y-axis
        arrowX.tag = "Xarrow";

        GameObject arrowY = Instantiate(arrowPrefabY, position + Vector3.up * offset, Quaternion.identity);
        arrowY.transform.rotation = Quaternion.Euler(-90, 0, 0); // Rotate arrowY -90 degrees around x-axis
        arrowY.tag = "Yarrow";

        GameObject arrowZ = Instantiate(arrowPrefabZ, position + Vector3.forward * offset, Quaternion.identity);
        arrowZ.tag = "Zarrow";
        // No rotation needed for arrowZ

        // Parent arrows to the selected object for organization (optional)
        GameObject arrowsParent = new GameObject("Arrows"); // Create a new empty GameObject as parent for arrows
        arrowX.transform.parent = arrowsParent.transform;
        arrowY.transform.parent = arrowsParent.transform;
        arrowZ.transform.parent = arrowsParent.transform;
        arrowsParent.transform.parent = selectedObject.transform; // Make arrows parented to the selected object
    }

    void DestroyArrows() {
        // Find the parent object that holds the arrows
        Transform arrowsParent = selectedObject.transform.Find("Arrows");
        if (arrowsParent != null) {
            // Destroy the parent object and all its children (the arrows)
            Destroy(arrowsParent.gameObject);
        }
    }

}
