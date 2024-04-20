using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFieldCalculator : MonoBehaviour {

    private const double permittivity = 8.854187817e-12; // Permittivity of free space

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            List<Vector3> fields = CalculateElectricFieldAtPoint();

            foreach (Vector3 field in fields) {
                Debug.Log(field);
            }
        }
    }

    public static List<Vector3> CalculateElectricFieldAtPoint() {
        List<Vector3> electricFields = new List<Vector3>();

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
            return electricFields;
        }

        // Iterate through all point charges in the scene
        foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()) {
            if (pointCharge.ChargeValue != 0) {
                // Iterate through all test charges
                foreach (PointCharge testCharge in testCharges) {
                    // Calculate electric field contribution for each test charge
                    Vector3 dif = testCharge.transform.position - pointCharge.transform.position;
                    double separationR = dif.magnitude;

                    double eFieldX = (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.x) / Mathf.Pow((float)separationR, 3);
                    double eFieldY = (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.y) / Mathf.Pow((float)separationR, 3);
                    double eFieldZ = (pointCharge.ChargeValue * pointCharge.ChargeMultiplier * dif.z) / Mathf.Pow((float)separationR, 3);

                    eFieldX *= (1 / (4 * Mathf.PI * permittivity));
                    eFieldY *= (1 / (4 * Mathf.PI * permittivity));
                    eFieldZ *= (1 / (4 * Mathf.PI * permittivity));

                    electricFields.Add(new Vector3((float)eFieldX, (float)eFieldY, (float)eFieldZ));
                }
            }
        }

        return electricFields;
    }
}
