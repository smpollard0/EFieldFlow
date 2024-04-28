using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElectricFieldCalculator : MonoBehaviour {
    public GameObject EFieldValuesMenuUI;
    public GameObject fieldVector;

    private const double permittivity = 8.854187817e-12; // Permittivity of free space
    private TMP_Text outputBox;
    private Toggle toggleVectorField;
    private GameObject boundingBoxCube;

    [System.Serializable]
    public class ElectricFieldResult {
        private Vector3 electricField;
        private int testUOID;

        public Vector3 ElectricField {
            get { return electricField; }
            set { electricField = value; }
        }

        public int TestUOID {
            get { return testUOID; }
            set { testUOID = value; }
        }
    }

    void Start() {
        outputBox = EFieldValuesMenuUI.transform.Find("ResultsBox").GetComponent<TMP_Text>();
        toggleVectorField = EFieldValuesMenuUI.transform.Find("Toggle").GetComponent<Toggle>();
    }

    void Update() {

        // Hide the efield menu if it's active on pause
        if (EFieldValuesMenuUI.activeSelf && PauseMEnu.GameIsPaused){
            EFieldValuesMenuUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !PauseMEnu.GameIsPaused){
            outputBox.text = "";
            List<ElectricFieldResult> fields = CalculateElectricFieldAtPoint();

            foreach (ElectricFieldResult field in fields) {
                outputBox.text += field.TestUOID.ToString() + ": ";
                outputBox.text += field.ElectricField.ToString() + " N/C" + "\n\n";
            }
            EFieldValuesMenuUI.SetActive(true);
        }



    }

    public static List<ElectricFieldResult> CalculateElectricFieldAtPoint() {
        List<ElectricFieldResult> electricFields = new List<ElectricFieldResult>();

        List<PointCharge> testCharges = new List<PointCharge>();
        // Find all test charges
        foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()) {
            if (pointCharge.ChargeValue == 0) {
                testCharges.Add(pointCharge);
            }
        }

        if (testCharges.Count == 0) {
            // Throw error message and don't continue executing
            Debug.LogError("No test charges found!");
            // Instead of throwing an error, we'll just set the text in the popup to show the error as text
            return electricFields;
        }

        // Iterate over the test charges
        // Iterate over all other charges
        // Add other charges' contributions at a particular test charge
        // Create vector3 and add it to the electricFields vector
        foreach (PointCharge testCharge in testCharges){
            ElectricFieldResult result = new ElectricFieldResult();
            double eFieldX = 0.0;
            double eFieldY = 0.0;
            double eFieldZ = 0.0;
            foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()) {
                if (pointCharge.ChargeValue != 0) {
                    Vector3 dif = testCharge.transform.position - pointCharge.transform.position;
                    double separationR = dif.magnitude;

                    eFieldX += (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.x) / Mathf.Pow((float)separationR, 3);
                    eFieldY += (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.y) / Mathf.Pow((float)separationR, 3);
                    eFieldZ += (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.z) / Mathf.Pow((float)separationR, 3);
                }
            }

            eFieldX *= (1 / (4 * Mathf.PI * permittivity));
            eFieldY *= (1 / (4 * Mathf.PI * permittivity));
            eFieldZ *= (1 / (4 * Mathf.PI * permittivity));

            result.ElectricField = new Vector3((float)eFieldX, (float)eFieldY, (float)eFieldZ);
            result.TestUOID = testCharge.UOID;
            electricFields.Add(result);
        }

        return electricFields;
    }

    public void CloseMenu(){
        EFieldValuesMenuUI.SetActive(false);
    }

    public void ToggleVectorField() {
    // Check if the toggle is checked (toggled on)
        if (toggleVectorField.isOn) {
            // Find minimum and maximum coordinates
            Vector3 minCoordinates = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxCoordinates = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()) {
                Vector3 position = pointCharge.transform.position;
                minCoordinates = Vector3.Min(minCoordinates, position);
                maxCoordinates = Vector3.Max(maxCoordinates, position);
            }

            // Calculate center point
            Vector3 centerPoint = (minCoordinates + maxCoordinates) / 2f;

            // Calculate dimensions
            Vector3 dimensions = maxCoordinates - minCoordinates;
            float maxDimension = Mathf.Max(dimensions.x, dimensions.y, dimensions.z);
            Vector3 cubeDimensions = new Vector3(maxDimension+10, maxDimension+10, maxDimension+10);

            // Create bounding box cube
            boundingBoxCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boundingBoxCube.name = "BoundingBoxCube"; // Set the name
            boundingBoxCube.transform.position = centerPoint;
            boundingBoxCube.transform.localScale = cubeDimensions;
        } else {
            // If the toggle is unchecked (toggled off), destroy the bounding box cube
            Destroy(boundingBoxCube);
        }
    }       
}
