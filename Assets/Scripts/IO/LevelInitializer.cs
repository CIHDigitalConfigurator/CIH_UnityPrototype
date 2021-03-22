using Newtonsoft.Json.Linq;
using OM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    public List<OM_level> levels;
    public bool levelsGenerated = false;


    void Start()
    {
        // Get tiles values from dictionary to array
        JArray tileArray = JArray.Parse(GetComponent<Reader>().jsonFolder["02_IN_Levels"].ToString());

        levels = new List<OM_level>();

        for (int i = 0; i < tileArray.Count; i++)
        {
            var height = 0.0f;
            if (i < tileArray.Count -1)
            {
                height = Math.Abs(tileArray[i + 1]["elevation"].ToObject<float>() - tileArray[i]["elevation"].ToObject<float>());
            }
            else
            {
                height = Math.Abs(tileArray[i]["elevation"].ToObject<float>() - tileArray[i-1]["elevation"].ToObject<float>());
            }

            OM_level level = new OM_level
            {
                Name = tileArray[i]["name"].ToObject<string>(),
                Elevation = tileArray[i]["elevation"].ToObject<float>(),
                Height = height
            };

            levels.Add(level);
        }

        levelsGenerated = true;

    }

}
