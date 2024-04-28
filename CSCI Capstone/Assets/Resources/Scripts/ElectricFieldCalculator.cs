using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElectricFieldCalculator : MonoBehaviour {
    public GameObject EFieldValuesMenuUI;

    private const double permittivity = 8.854187817e-12; // Permittivity of free space
    private TMP_Text outputBox;

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
        // Initialize outputBox in Start method
        outputBox = EFieldValuesMenuUI.transform.Find("ResultsBox").GetComponent<TMP_Text>();
    }

    void Update() {

        // Hide the efield menu if it's active
        if (EFieldValuesMenuUI.activeSelf && PauseMEnu.GameIsPaused){
            EFieldValuesMenuUI.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && !PauseMEnu.GameIsPaused){
            outputBox.text = "";
            List<ElectricFieldResult> fields = CalculateElectricFieldAtPoint();

            foreach (ElectricFieldResult field in fields) {
                outputBox.text += field.TestUOID.ToString() + ": ";
                outputBox.text += field.ElectricField.ToString() + " N/C" + "\n";
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
}
