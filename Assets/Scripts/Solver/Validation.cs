using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Validation : MonoBehaviour
{
    public string validationMessage;
    public float minArea { get; set; }
    public float area { get; set; }


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

    public void PrintMessagesOnScrollPanel(GameObject scrollPanel)
    {

    }

    public static void CompareAreas(GameObject obj)
        
    {
        
        JArray typeArray = JArray.Parse(GameObject.FindObjectOfType<UIController>().jsonReader.GetComponent<Reader>().jsonFolder["02_IN_RoomTypes"].ToString());

        foreach (JToken typeData in typeArray)
        {

            string rName = typeData["name"].ToString().ToUpper();
            if (rName == obj.GetComponent<EG_room>().Type)
            {

                if (float.Parse(typeData["min_area"].ToString()) > obj.GetComponent<EG_room>().Area) GameObject.FindObjectOfType<EffectsManager>().AddTexture(obj);
                break;
            }


        }
    }
}
