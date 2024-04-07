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

        // Come back and add one for charge value
        // Subscribe to input field click events
        xCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });
        yCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });
        zCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });

    }

    void OnEditStart(){
        isEditing = true;
    }

    // Called once every frame
    void Update() {
        // If selectedObject is not null, we want to make this menu visible
        if (ObjectInteraction.selectedObject != null && !isEditing) {
            ShowMenu();
        } else if (ObjectInteraction.selectedObject != null && isEditing) {
            selectionMenuUI.SetActive(true);
        } else {
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
        // First check if all the input data is valid (i.e. they're numbers)

        // Update object position with input field values
        temporaryPosition.x = float.Parse(xCoordinateField.text);
        temporaryPosition.y = float.Parse(yCoordinateField.text);
        temporaryPosition.z = float.Parse(zCoordinateField.text);
        ObjectInteraction.selectedObject.transform.position = temporaryPosition;
        isEditing = false;
    }
}