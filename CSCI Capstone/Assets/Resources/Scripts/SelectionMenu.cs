using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionMenu : MonoBehaviour {

    public GameObject selectionMenuUI;

    private TMP_InputField xCoordinateField;
    private TMP_InputField yCoordinateField;
    private TMP_InputField zCoordinateField;
    private TMP_InputField chargeField;
    private TMP_Text errorBox;

    // Boolean to prevent accidentally deleting selected object
    public static bool isEditing = false;

    private Vector3 temporaryPosition;

    // Called before update
    void Start() {
        // Position text fields
        xCoordinateField = selectionMenuUI.transform.Find("xPos").GetComponent<TMP_InputField>();
        yCoordinateField = selectionMenuUI.transform.Find("yPos").GetComponent<TMP_InputField>();
        zCoordinateField = selectionMenuUI.transform.Find("zPos").GetComponent<TMP_InputField>();

        chargeField = selectionMenuUI.transform.Find("ChargeValueBox").GetComponent<TMP_InputField>();
        errorBox = selectionMenuUI.transform.Find("ErrorText").GetComponent<TMP_Text>();

        // Add listeners to input field click events
        xCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });
        yCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });
        zCoordinateField.onSelect.AddListener(delegate { OnEditStart(); });
        chargeField.onSelect.AddListener(delegate { OnEditStart(); });
    }

    void OnEditStart(){
        isEditing = true;
    }

    // Called once every frame
    void Update() {
        // If selectedObject is not null, make menu visible
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

        // Come back and add charge value once that class structure is set up
        chargeField.text = 0.0.ToString();
        // chargeField.text = something.ToString();
    }

    public void SaveChanges(){
        errorBox.text = ""; // Initialize error box text to empty string
        // First check if all the input data is valid (i.e. they're numbers)
        if (!IsValidFloat(xCoordinateField.text)) {
            ShowErrorMessage("Invalid X coordinate.");
            return;
        }

        if (!IsValidFloat(yCoordinateField.text)) {
            ShowErrorMessage("Invalid Y coordinate.");
            return;
        }

        if (!IsValidFloat(zCoordinateField.text)) {
            ShowErrorMessage("Invalid Z coordinate.");
            return;
        }

        if (!IsValidFloat(chargeField.text)) {
            ShowErrorMessage("Invalid charge value.");
            return;
        }

        // Update object position with input field values
        temporaryPosition.x = float.Parse(xCoordinateField.text);
        temporaryPosition.y = float.Parse(yCoordinateField.text);
        temporaryPosition.z = float.Parse(zCoordinateField.text);
        // set object's charge value
        ObjectInteraction.selectedObject.transform.position = temporaryPosition;
        isEditing = false;
    }

    bool IsValidFloat(string value) {
        float result;
        return float.TryParse(value, out result);
    }

    void ShowErrorMessage(string message) {
        // Show error message in TMP text box
        errorBox.text = message;
    }
}