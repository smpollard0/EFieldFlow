using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFieldCalculator : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            CalculateElectricFieldAtPoint(Vector3.zero);
        }
    }

    public static Vector3 CalculateElectricFieldAtPoint(Vector3 point) {
        Vector3 electricField = Vector3.zero;

        // Iterate through all point charges in the scene
        foreach (PointCharge pointCharge in FindObjectsOfType<PointCharge>()){
            // Calculate the distance and direction from the point charge to the point
            // Vector3 direction = point - pointCharge.transform.position;
            // float distance = direction.magnitude;

            // // Check if the distance is not too close to avoid division by zero
            // if (distance > 0.01f) {
            //     // Calculate the electric field contribution from the point charge
            //     float fieldMagnitude = pointCharge.ChargeValue / (distance * distance);
            //     electricField += fieldMagnitude * direction.normalized;
            // }

            Debug.Log(pointCharge.ChargeValue);
        }

        return electricField;
    }
}
