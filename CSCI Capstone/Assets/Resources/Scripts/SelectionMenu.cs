using System;
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
    private TMP_Text objectTitle;
    private TMP_Dropdown dropdownMenu; 

    // Boolean to prevent accidentally deleting selected object
    public static bool isEditing = false;

    private Vector3 temporaryPosition;

    // Mapping of dropdown options to numerical values
    private Dictionary<string, int> dropdownOptionsExponents = new Dictionary<string, int>() {
        { "kC", 3 },
        { "C", 0 },
        { "mC", -3 },
        { "μC", -6 },
        { "nC", -9 },
        { "pC", -12 }
    };

    // Called before update
    void Start() {
        // Position text fields
        xCoordinateField = selectionMenuUI.transform.Find("xPos").GetComponent<TMP_InputField>();
        yCoordinateField = selectionMenuUI.transform.Find("yPos").GetComponent<TMP_InputField>();
        zCoordinateField = selectionMenuUI.transform.Find("zPos").GetComponent<TMP_InputField>();

        chargeField = selectionMenuUI.transform.Find("ChargeValueBox").GetComponent<TMP_InputField>();
        errorBox = selectionMenuUI.transform.Find("ErrorText").GetComponent<TMP_Text>();
        objectTitle = selectionMenuUI.transform.Find("ObjectTitle").GetComponent<TMP_Text>();

        dropdownMenu = selectionMenuUI.transform.Find("ChargePrefix").GetComponent<TMP_Dropdown>();

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
        PointCharge pointChargeComponent = ObjectInteraction.selectedObject.GetComponent<PointCharge>();

        xCoordinateField.text = objectPosition.x.ToString();
        yCoordinateField.text = objectPosition.y.ToString();
        zCoordinateField.text = objectPosition.z.ToString();

        chargeField.text = pointChargeComponent.ChargeValue.ToString();

        objectTitle.text = "Object " + pointChargeComponent.UOID;

        // Populate the dropdown menu with options
        dropdownMenu.ClearOptions();
        dropdownMenu.AddOptions(new List<string>(dropdownOptionsExponents.Keys));

        // Set the dropdown value based on the clicked object's chargeMultiplier
        string currentOption = GetDropdownOption(ObjectInteraction.selectedObject.GetComponent<PointCharge>().ChargeMultiplier);
        dropdownMenu.value = dropdownMenu.options.FindIndex(option => option.text == currentOption);
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
        // Set object's charge value
        ObjectInteraction.selectedObject.GetComponent<PointCharge>().ChargeValue = float.Parse(chargeField.text);

        // ObjectInteraction.selectedObject.
        ObjectInteraction.selectedObject.transform.position = temporaryPosition;
        isEditing = false;
    }

    bool IsValidFloat(string value) {
        float result;
        return float.TryParse(value, out result);
    }

    // Show error message in TMP text box
    void ShowErrorMessage(string message) {
        errorBox.text = message;
    }

    // Helper method to get the dropdown option corresponding to a numerical value
    private string GetDropdownOption(double value) {
        foreach (var pair in dropdownOptionsExponents) {
            if (Math.Pow(10, pair.Value) == value) {
                return pair.Key;
            }
        }
        return string.Empty;
    }

    // Public method for the UI to update when the dropdown value is changed
    public void OnDropdownValueChanged(int index) {
        // Get the selected option from the dropdown
        string selectedOption = dropdownMenu.options[index].text;

        // Get the exponent associated with the selected option
        int exponent = dropdownOptionsExponents[selectedOption];

        // Calculate the numerical value using powers of 10
        double numericalValue = Math.Pow(10, exponent);

        // Set the chargeMultiplier of the clicked object
        ObjectInteraction.selectedObject.GetComponent<PointCharge>().ChargeMultiplier = numericalValue;
    }
}