using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Reader : MonoBehaviour
{

    #region Public Variables
    public List<string> fileNames = new List<string>();

    public Dictionary<string, JArray> jsonFolder =
new Dictionary<string, JArray>();

    #endregion

    #region Private Variables
    // For now this 
    string relativeJsonpath = @"\Mott MacDonald\Platform Design Programme - 0.10 Digital Configurator\WP5 Reference Implementation\5.7 Implementation\02_Design Configurators\Objects\";
    string homePath;
    #endregion

    void Awake()
    {
        homePath = Environment.GetEnvironmentVariable("HOMEPATH");

        // Read files from Sharepoint and save to variable
        for (int i = 0; i < fileNames.Count; i++)
        {
            StreamReader streamReader = new StreamReader(homePath + relativeJsonpath + fileNames[i]);

            JArray json = JArray.Parse(streamReader.ReadToEnd().ToString());
            jsonFolder.Add(fileNames[i].Split('.')[0], json);

            streamReader.Close();
        }
    }
}
