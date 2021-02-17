using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Validation : MonoBehaviour
{
    public string validationMessage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTextToValidation(string rType, string name, float minSize, float area)
    {
        string message = "Room: " + name + " of type: " + rType + " has area " + (Math.Round(area)).ToString() + " m2";
        if (area >= minSize)
        {
            message = message + "; minimum area: " + (Math.Round(minSize)).ToString() + "m2 // passed";

        }
        else
        {
            message = message + " but the minimum area should be: " + (Math.Round(minSize)).ToString() + "m2 // failed";
        }

        validationMessage = validationMessage + "\n\n" + message;
    }
}
