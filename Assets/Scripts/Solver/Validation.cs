using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Validation : MonoBehaviour
{
    public string validationMessage;
    //public float minArea { get; set; }
    //public float area { get; set; }




    private void AddTextToValidation(string message)
    {
        validationMessage =  message + "\n\n" + validationMessage;
    }

    public void PrintMessagesOnScrollPanel(GameObject scrollPanel)
    {

    }

    public void CompareAreas(GameObject obj)
        
    {
        
        JArray typeArray = JArray.Parse(GameObject.FindObjectOfType<UIController>().jsonReader.GetComponent<Reader>().jsonFolder["02_IN_RoomTypes"].ToString());
        float minArea = 0;
        string rType = "";
        foreach (JToken typeData in typeArray)
        {

            string temprType = typeData["name"].ToString().ToUpper();
            if (temprType == obj.GetComponent<EG_room>().Type)
            {
                minArea = float.Parse(typeData["min_area"].ToString());
                rType = temprType;
                if (minArea > obj.GetComponent<EG_room>().Area) GameObject.FindObjectOfType<EffectsManager>().AddTexture(obj);
                break;
            }


        }

        string message = "Room: " + obj.name + " of type: " + obj.GetComponent<EG_room>().Type.ToUpper() + " has area " + (Math.Round(obj.GetComponent<EG_room>().Area)).ToString() + " m2";
        if (obj.GetComponent<EG_room>().Area >= minArea)
        {
            message = message + "; minimum area: " + (Math.Round(minArea)).ToString() + "m2 // <b>PASSED</b>";

        }
        else
        {
            message = message + " but the minimum area should be: " + (Math.Round(minArea)).ToString() + "m2 // <b>FAILED</b>";
        }

        AddTextToValidation(message);
    }
}
