﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using Newtonsoft.Json.Linq;

/// <summary>
/// General functionality associated with UI - we can also rely on the EventSystem native scripts if that's the preference - they are currently in and are required for the UI to work, but can cut them down
/// 
/// TODO UI wise:
/// - Add cancel button to the input field
/// - Introduce button block on pause and test all UI flows
/// - Click away from error screen
/// </summary>

public class UIController : MonoBehaviour
{
    #region Private Variables
    MovementController movementController;
    RaycastController raycastController;
    VisibilityController visibilityController;
    //CameraViewsControler cameraViewsControler;
    CameraController cameraController;
    
    #endregion

    #region Public Variables
    public Canvas mainCanvas;
    public GameObject buttonPrefab;
    public GameObject togglePrefab;
    public GameObject errorScreen;
    public GameObject jsonReader;
    public GameObject scrollPanel;
    public GameObject jsonWriter;


    #endregion
    void Awake() 
    {
        movementController = gameObject.GetComponent<MovementController>();
        raycastController = gameObject.GetComponent<RaycastController>();
        cameraController = Camera.main.GetComponent<CameraController>();

        //cameraViewsControler = gameObject.GetComponent<CameraViewsControler>();

        SpawnButtons();
        SpawnCameraButton();
        SpawnLevelButtons();

    }

    void Start()
    {
        visibilityController = gameObject.GetComponent<VisibilityController>();
    }

    #region Buttons Spawning
    private void SpawnCameraButton()
    {
        GameObject button = Instantiate(togglePrefab);
        button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
        button.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, 300f);

        button.GetComponentInChildren<Text>().text = "2D";
        button.GetComponent<Toggle>().onValueChanged.AddListener((value) => Toggle2D3D(value));
        button.GetComponent<Toggle>().isOn = false;
    }

    private void SpawnLevelButtons()
    {
        // Get level values from dictionary to array
        JArray levelData = JArray.Parse(jsonReader.GetComponent<Reader>().jsonFolder["02_IN_Levels"].ToString());

        for (int i = 0; i < levelData.Count; i++)
        {
            GameObject button = Instantiate(togglePrefab);
            button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
            button.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            button.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, 50f + i*25);

            var name = "Level " + levelData[i]["name"].ToString();

            button.GetComponentInChildren<Text>().text = name;
            button.GetComponent<Toggle>().onValueChanged.AddListener((value) => LayerToggle(value, name));
        }
    }
    private void SpawnButtons()
    {

        // Get tiles values from dictionary to array
        JArray roomArray = JArray.Parse(jsonReader.GetComponent<Reader>().jsonFolder["02_IN_RoomTypes"].ToString());

        int i = 0;
        foreach (JToken tileData in roomArray)
        {
            string rName = tileData["name"].ToString().ToUpper();
            float minSize = float.Parse(tileData["min_area"].ToString());
            bool circ = tileData["circulation"].ToString() == "True" ? true : false;
            Color rColour = Random.ColorHSV(0f, 1f, 0.4f, 0.7f, 0.5f, 1f);
            Color rColourO = Random.ColorHSV(0f, 1f, 0.4f, 0.7f, 0.5f, 1f);
            InstantiateButton(rName, 100f, -50 - i * 35, minSize, rColour, rColourO, circ);
            i++;
        }

        InstantiateVoidButton(100f, -50 - i * 35);

    }

    #endregion
    #region Button Triggered Functions
    public void SendToJson()
    {
        jsonWriter.GetComponent<Writer>().WriteJson();
    }

    public void Toggle2D3D(bool value) 
    {
        cameraController.Toggle2D3D(value);
    }

    public void PauseGame()
    {
        /// <summary>
        /// Adds functionality associated with pausing - an example of enabling a blocking image on canvas here
        /// </summary>
        if (mainCanvas.GetComponent<Image>().enabled)
        {
            mainCanvas.GetComponent<Image>().enabled = false;
            mainCanvas.GetComponentInChildren<Text>().text = "Pause";
            movementController.MovBlocked = false ? movementController.CurrentObject != null : true;
            raycastController.RayBlocked = false;

        }
        else
        {
            mainCanvas.GetComponent<Image>().enabled = true;
            mainCanvas.GetComponentInChildren<Text>().text = "Restart";
            movementController.MovBlocked = true;
            raycastController.RayBlocked = true;
        }

    }

    public void Validate()
    {
        //GameObject panel = Instantiate(panelPrefab, mainCanvas.transform, false);
        if (!scrollPanel.activeSelf) scrollPanel.SetActive(true);
        Validation validation = gameObject.GetComponent<Validation>();
        scrollPanel.GetComponentInChildren<Text>().text = validation.validationMessage;
        movementController.EdgeTypesWriter();

    }
    public IEnumerator EnableInputField(float delay) 
    {
        yield return new WaitForSeconds(delay);
        // enable background image
        mainCanvas.GetComponent<Image>().enabled = true;

        //enable input field
        mainCanvas.GetComponentInChildren<InputField>(true).gameObject.SetActive(true);

    }

    public IEnumerator DisplayWarning(float duration, string warning) 
    {
        // enable background image
        mainCanvas.GetComponent<Image>().enabled = true;

        // enable warning screen
        errorScreen.GetComponentInChildren<Text>().text = warning;
        errorScreen.SetActive(true);
        yield return new WaitForSeconds(duration);
        errorScreen.SetActive(false);
        mainCanvas.GetComponent<Image>().enabled = false;

    }

    #endregion
    #region Utilities
    private void LayerToggle(bool isOn, string name)
    {
        if (isOn)
        {
            //layervisibilityController.LayerCullingShow(Camera.main, name);
            visibilityController.LevelShow(name);
        }
        else
        {
            //layervisibilityController.LayerCullingHide(Camera.main, name);
            visibilityController.LevelHide(name);

        }
    }

    private void InstantiateButton(string rName, float posX, float posY, float minSize, Color rColour, Color rColourO, bool circ)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);

        button.GetComponentInChildren<Text>().text = rName;

        // if the size was to be altered
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeH);
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeV);

        // add the create room function
        button.GetComponent<Button>().onClick.AddListener(() => CallCreateRoom(rName, minSize, rColour, rColourO, circ));

    }


    private void InstantiateVoidButton(float posX, float posY)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);

        button.GetComponentInChildren<Text>().text = "EXTERIOR";

        // add the create room function
        button.GetComponent<Button>().onClick.AddListener(() => CallCreateVoid());

    }

    private void CallCreateRoom(string rName, float minSize, Color rColour, Color rColourO, bool circ)
    {

        // create room
        movementController.CreateRoom(rName, minSize, rColour, rColourO, circ);

    }

    private void CallCreateVoid()
    {

        // create room
        movementController.CreateVoid();

    }
    #endregion
    ///<remarks>
    ///NW
    ///[MenuItem("AssetDatabase/LoadAllAssetsAtPath")]
    ///If we want to instantiate anything from the project folder that is not in the game use this method, just create a resources folder with the prefabs
    ///</remarks>

}
