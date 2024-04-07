using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour{

    public GameObject pointCharge;
    private Camera mainCamera; // Reference to the main camera
    private float distanceFromCamera = 10f; // variable for how far from the camera to put an object if it doesn't collide with something
    bool buttonIsPressed = false;

    void Awake(){
        // reference the camera (this feels dumb considering this script is attached to the camera)
        mainCamera = Camera.main;
    }

    void Update() {
        // press Q button to play a game object
        if (Input.GetKey(KeyCode.Q) && !buttonIsPressed && !PauseMEnu.GameIsPaused && !SelectionMenu.isEditing){
            // create gameobject to place
            GameObject objectToPlace;
            
            // cast a ray from the camera through the mouse cursor position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // switch statement to check what kind of object to create based on what is selected in the toolbar
            switch (UiController.selectedItemIndex){
                // point charges
                case 0:
                    // temporarily place spheres; come back later and make these prefabs 
                    objectToPlace = GameObject.Instantiate(pointCharge);
                    objectToPlace.tag = "Sphere"; // give spheres the "Sphere" tag so they are recognized when being clicked
                    if (Physics.Raycast(ray, out hit)){
                        //  when the ray intersects with something in the scene, place object
                        SphereCollider sphereCollider = objectToPlace.GetComponent<SphereCollider>();
                        if (sphereCollider != null){ // should probably have error checking if this is null for some reason
                            float sphereRadius = sphereCollider.radius;
                            Vector3 surfaceOffset = hit.normal * sphereRadius;
                            // set position of object to where the ray hits a collider plus a radius offset so the sphere isn't inside the collided object
                            objectToPlace.transform.position = hit.point + surfaceOffset;
                        }
                    }
                    else {
                        // when the ray doesn't intersect with anything in the scene, place the object a fixed distance from the camera
                        objectToPlace.transform.position = ray.origin + ray.direction * distanceFromCamera;
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
        }// if
        if (Input.GetKeyUp(KeyCode.Q)){
            // Reset the flag when the button is released
            buttonIsPressed = false;
        }// if
    }// Update()
}// class PlaceObjects

