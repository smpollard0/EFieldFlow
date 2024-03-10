using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjects : MonoBehaviour
{
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
        if (Input.GetMouseButtonDown(0)) {
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

                    // This will create the arrow prefabs at the object
                    InstantiateArrows(newSelection.transform.position);
                }
            }
        }
    }

    void InstantiateArrows(Vector3 position) {
        // Instantiate arrow objects in the x, y, and z directions
        // CURRENT: CREATE ARROW PREFABS IN BLENDER AND IMPORT THEM INTO UNITYs
        GameObject arrowX = Instantiate(arrowPrefabX, position, Quaternion.identity);
        GameObject arrowY = Instantiate(arrowPrefabY, position, Quaternion.identity);
        GameObject arrowZ = Instantiate(arrowPrefabZ, position, Quaternion.identity);

        // Parent arrows to the selected object for organization (optional)
        arrowX.transform.parent = selectedObject.transform;
        arrowY.transform.parent = selectedObject.transform;
        arrowZ.transform.parent = selectedObject.transform;
    }
}
