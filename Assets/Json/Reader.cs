﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Reader : MonoBehaviour
{

    #region Public Variables
    public List<string> fileNames = new List<string>();

    [HideInInspector]
    public int jsonCount;
    public List<JObject> jsons = new List<JObject>();
    #endregion

    #region Private Variables
    // For now this 
    string relativeJsonpath = @"\Mott MacDonald\Platform Design Programme - 0.10 Digital Configurator\WP5 Reference Implementation\5.7 Implementation\Assets\Interop Data\";
    string homePath;
    #endregion

    void Start()
    {
        homePath = Environment.GetEnvironmentVariable("HOMEPATH");

        // Read files from Sharepoint and save to variable
        for (int i = 0; i < fileNames.Count; i++)
        {
            StreamReader streamReader = new StreamReader(homePath + relativeJsonpath + fileNames[i]);
            jsons.Add(JObject.Parse(streamReader.ReadToEnd()));
            streamReader.Close();
        }

        jsonCount = jsons.Count;
    }
}