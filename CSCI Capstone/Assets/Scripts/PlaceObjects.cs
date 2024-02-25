using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour{

    private Camera mainCamera; // Reference to the main camera
    
    bool buttonIsPressed = false;

    void Awake(){
        // create reference to main camera
        mainCamera = Camera.main;
    }

    void Update() {
        // press Q button to play a game object
        if (Input.GetKey(KeyCode.Q) && !buttonIsPressed){
            // Cast a ray from the camera through the mouse cursor position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
                // create gameobject to place
                GameObject objectToPlace;
                // switch statement to check what kind of object to create based on what is selected in the toolbar
                switch (UiController.selectedItemIndex){
                    // point charges
                    case 0:
                        Debug.Log("Point Charge Selected");
                        // temporarily place spheres; come back later and make these prefabs 
                        objectToPlace = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        // Object placement when the ray intersects with something in the scene
                        SphereCollider sphereCollider = objectToPlace.GetComponent<SphereCollider>();
                        if (sphereCollider != null){
                            float sphereRadius = sphereCollider.radius;
                            Vector3 surfaceOffset = hit.normal * sphereRadius;
                            // need to cast a ray from the camera to the position of the mouse to place object at mouse location
                            objectToPlace.transform.position = hit.point + surfaceOffset;
                        }
                        buttonIsPressed = true;
                        break;

                    // for the time being, this is commented out because I don't have proper parameters for line or volume charges
                    // line charges
                    // case 1:
                    //     Debug.Log("Line Charge Selected");
                    //     objectToPlace = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    //     objectToPlace.transform.position = new Vector3(0, 3f, 0);
                    //     buttonIsPressed = true;
                    //     break;
                    // // volume charges
                    // case 2:
                    //     Debug.Log("Volume Charge Selected");
                    //     objectToPlace = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //     objectToPlace.transform.position = new Vector3(0, 3f, 0);
                    //     buttonIsPressed = true;
                    //     break;
                    default:
                        // this should never happen
                        break;
                }// switch
            }
        }
        if (Input.GetKeyUp(KeyCode.Q)){
            // Reset the flag when the button is released
            buttonIsPressed = false;
        }
    }
}

