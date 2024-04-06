using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionMenu : MonoBehaviour {

    public GameObject selectionMenuUI;

    private TMP_InputField xCoordinateField;
    private TMP_InputField yCoordinateField;
    private TMP_InputField zCoordinateField;

    // Boolean to prevent accidentally deleting selected object
    public static bool isEditing = false;

    private Vector3 temporaryPosition;

    // Called at the start?
    void Start() {
        xCoordinateField = selectionMenuUI.transform.Find("xPos").GetComponent<TMP_InputField>();
        yCoordinateField = selectionMenuUI.transform.Find("yPos").GetComponent<TMP_InputField>();
        zCoordinateField = selectionMenuUI.transform.Find("zPos").GetComponent<TMP_InputField>();

        // Subscribe to input field click events
        xCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });
        yCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });
        zCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });

        // Subscribe to input field end edit events
        xCoordinateField.onEndEdit.AddListener(delegate { OnEditEnd(); });
        yCoordinateField.onEndEdit.AddListener(delegate { OnEditEnd(); });
        zCoordinateField.onEndEdit.AddListener(delegate { OnEditEnd(); });
    }

    void OnEditStart(){
        isEditing = true;
        Debug.Log("Field clicked");
    }

    void OnEditEnd(){
        isEditing = false;
    }

    void UpdateObjectPosition() {
        // Get the current object position
        Vector3 objectPosition = ObjectInteraction.selectedObject.transform.position;

        // Update object position with input field values
        objectPosition.x = float.Parse(xCoordinateField.text);
        objectPosition.y = float.Parse(yCoordinateField.text);
        objectPosition.z = float.Parse(zCoordinateField.text);

        // Apply the new position to the object
        ObjectInteraction.selectedObject.transform.position = objectPosition;
    }

    // Called once every frame
    void Update() {
        // If selectedObject is not null, we want to make this menu visible
        if (ObjectInteraction.selectedObject != null) {
            ShowMenu();
        } else{
            selectionMenuUI.SetActive(false);
        }
    }

    void ShowMenu() {
        // Make the UI visible
        selectionMenuUI.SetActive(true);

        // Populate text fields with selectedObject's parameters
        Vector3 objectPosition = ObjectInteraction.selectedObject.transform.position;

        xCoordinateField.text = objectPosition.x.ToString();
        yCoordinateField.text = objectPosition.y.ToString();
        zCoordinateField.text = objectPosition.z.ToString();
    }

    public void SaveChanges(){
        Debug.Log("test");
    }
}