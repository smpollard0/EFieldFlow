using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCharge : MonoBehaviour {
    // Static variable to keep track of the number of objects created
    private static int nextUOID = 0;

    // Private member variables
    [SerializeField] private int uoid; // Unique Object Identifier
    [SerializeField] private double chargeValue;
    [SerializeField] private double chargeMultiplier;

    // Awake method is called when the script instance is being loaded
    private void Awake() {
        // Set the uoid for this instance and increment nextUOID
        uoid = nextUOID++;
    }

    // Default constructor
    public PointCharge() {
        // Set default values for chargeValue and chargeMultiplier
        chargeValue = 0.0;
        chargeMultiplier = 1.0;
    }

    // Public properties with getters and setters
    public double ChargeValue {
        get { return chargeValue; }
        set { chargeValue = value; }
    }

    public double ChargeMultiplier {
        get { return chargeMultiplier; }
        set { chargeMultiplier = value; }
    }

    public int UOID {
        get { return uoid; }
        set { uoid = value; }
    }

}