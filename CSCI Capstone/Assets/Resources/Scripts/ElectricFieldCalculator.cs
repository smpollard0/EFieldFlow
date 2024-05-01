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
    private Vector3 minBoundingBox;
    private Vector3 maxBoundingBox;
    private float fieldDensity = 1.0f;

    private List<GameObject> instantiatedVectors = new List<GameObject>();


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

    /*
    This function is slightly different than the one above. For the time being, I opted to create
    a new function that is used for calculating the individual vectors within some bounding box
    region so I don't risk breaking the above function for the normal E-Field vector calculation.
    */
    public Vector3 CalculateElectricFieldAtPoint(Vector3 position){
        Vector3 electricField = Vector3.zero;

        foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()) {
            Vector3 dif = position - pointCharge.transform.position;
            double separationR = dif.magnitude;

            double eFieldX = (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.x) / Mathf.Pow((float)separationR, 3);
            double eFieldY = (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.y) / Mathf.Pow((float)separationR, 3);
            double eFieldZ = (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.z) / Mathf.Pow((float)separationR, 3);

            eFieldX *= (1 / (4 * Mathf.PI * permittivity));
            eFieldY *= (1 / (4 * Mathf.PI * permittivity));
            eFieldZ *= (1 / (4 * Mathf.PI * permittivity));

            electricField += new Vector3((float)eFieldX, (float)eFieldY, (float)eFieldZ);
        }

        return electricField;
    }

    public void CloseMenu(){
        EFieldValuesMenuUI.SetActive(false);
    }

    public void ToggleVectorField() {
        // Check if the toggle is checked (toggled on)
        if (toggleVectorField.isOn) {
            // Find minimum and maximum coordinates
            CalculateBoundingBox();

            // Calculate center point
            Vector3 centerPoint = (minBoundingBox + maxBoundingBox) / 2f;

            // Calculate average position of test charges
            Vector3 averageNonTestChargePosition = Vector3.zero;
            int count = 0;
            foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()) {
                if (pointCharge.ChargeValue != 0) {
                    averageNonTestChargePosition += pointCharge.transform.position;
                    count++;
                }
            }
            averageNonTestChargePosition /= count;

            // Offset center point by the average position of test charges
            centerPoint += averageNonTestChargePosition;

            // Calculate dimensions
            Vector3 dimensions = maxBoundingBox - minBoundingBox;
            float maxDimension = Mathf.Max(dimensions.x, dimensions.y, dimensions.z);
            maxDimension += 5;

            // I am skeptical of this...
            for (float x = centerPoint.x - maxDimension / 2; x <= centerPoint.x + maxDimension / 2; x += fieldDensity) {
                for (float y = centerPoint.y - maxDimension / 2; y <= centerPoint.y + maxDimension / 2; y += fieldDensity) {
                    for (float z = centerPoint.z - maxDimension / 2; z <= centerPoint.z + maxDimension / 2; z += fieldDensity) {
                        Vector3 position = new Vector3(x, y, z);
                        Vector3 electricField = CalculateElectricFieldAtPoint(position);
                        InstantiateVectorPrefab(position, electricField);
                    }
                }
            }

            
            
            
        } else {
            // If the toggle is unchecked (toggled off), destroy the bounding box cube
            foreach (GameObject vector in instantiatedVectors) {
                Destroy(vector);
            }
            instantiatedVectors.Clear(); // Clear the list
        }
    }

    /*
    This function calculates the particular region where I want to render the vector
    field.
    */
    void CalculateBoundingBox() {
        minBoundingBox = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        maxBoundingBox = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()) {
            Vector3 position = pointCharge.transform.position;
            minBoundingBox = Vector3.Min(minBoundingBox, position);
            maxBoundingBox = Vector3.Max(maxBoundingBox, position);
        }
    }

    /*
    This function uses the electric field vector prefab and instantiates it and rotates it
    to the proper rotation.
    */
    void InstantiateVectorPrefab(Vector3 position, Vector3 electricField) {
        // Instantiate the vector prefab at the given position
        GameObject vectorPrefabInstance = Instantiate(fieldVector, position, Quaternion.identity);

        // Calculate rotation to align the vector with the electric field direction
        Quaternion rotation = Quaternion.LookRotation(electricField.normalized);

        // Set the rotation of the instantiated prefab
        vectorPrefabInstance.transform.rotation = rotation;

        // Add the instantiated vector to the list
        instantiatedVectors.Add(vectorPrefabInstance);
    }
}
