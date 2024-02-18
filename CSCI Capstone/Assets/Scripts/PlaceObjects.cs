using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour{

    bool buttonIsPressed = false;

    void Update(){
        if (Input.GetKey(KeyCode.Q) && !buttonIsPressed){
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(0, 3f, 0);
            buttonIsPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Q)){
            // Reset the flag when the button is released
            buttonIsPressed = false;
        }
    }
}

