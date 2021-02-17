using System.Collections;
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
    LayerVisibilityController layervisibilityController;
    VisibilityController visibilityController;
    CameraViewsControler cameraViewsControler;
    
    #endregion

    #region Public Variables
    public Canvas mainCanvas;
    public GameObject buttonPrefab;
    public GameObject togglePrefab;
    public GameObject errorScreen;
    public GameObject jsonReader;
    public GameObject panelPrefab;
    public GameObject jsonWriter;


    #endregion
    void Awake() 
    {
        movementController = gameObject.GetComponent<MovementController>();
        raycastController = gameObject.GetComponent<RaycastController>();
        layervisibilityController = Camera.main.GetComponent<LayerVisibilityController>();

        cameraViewsControler = gameObject.GetComponent<CameraViewsControler>();

        SpawnButtons();
        SpawnCameraButton();
        SpawnLevelButtons();

    }

    void Start()
    {
        visibilityController = gameObject.GetComponent<VisibilityController>();
    }


    public void SendToJson()
    {
        jsonWriter.GetComponent<Writer>().WriteJson();
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
        
        GameObject panel = Instantiate(panelPrefab);
        panel.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);

        GameObject panelText = new GameObject("validation message");
        panelText.transform.SetParent(panel.transform);
        var textComponent = panelText.AddComponent<Text>();

        textComponent.GetComponent<RectTransform>().anchorMin = panel.GetComponent<RectTransform>().anchorMin;
        textComponent.GetComponent<RectTransform>().anchorMax = panel.GetComponent<RectTransform>().anchorMax;
        textComponent.GetComponent<RectTransform>().anchoredPosition = panel.GetComponent<RectTransform>().anchoredPosition;
        
        Validation v = gameObject.GetComponent<Validation>();
        textComponent.text = v.validationMessage;

        var font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        textComponent.font = font;

        movementController.EdgeTypesWriter();
        // print message from validation class
    }
    
    private void SpawnCameraButton()
    {
        GameObject button = Instantiate(togglePrefab);
        button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
        button.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -400f);

        button.GetComponentInChildren<Text>().text = "2D";
        button.GetComponent<Toggle>().onValueChanged.AddListener((value) => cameraViewsControler.Toggle2D3D(value));
    }

    private void SpawnLevelButtons()
    {
        // Get level values from dictionary to array
        JArray levelData = JArray.Parse(jsonReader.GetComponent<Reader>().jsonFolder["level"]["level"].ToString());

        for (int i = 0; i < levelData.Count; i++)
        {
            GameObject button = Instantiate(togglePrefab);
            button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
            button.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            button.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -200f - i*25);

            var name = "Level " + levelData[i]["name"].ToString();

            button.GetComponentInChildren<Text>().text = name;
            button.GetComponent<Toggle>().onValueChanged.AddListener((value) => LayerToggle(value, name));
        }
    }

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


    private void SpawnButtons() 
    {
        string path = "Assets/Json/room_types.json";
        StreamReader a = new StreamReader(path);
        JObject json = JObject.Parse(a.ReadToEnd());
        //int roomCount = json.Count;
        for (int i = 0; i < json.Count; i++) 
        {
            //Debug.Log(json[i.ToString()]["name"]);
            string rName = json[i.ToString()]["name"].ToString().ToUpper();
            float minSize = float.Parse(json[i.ToString()]["min_area"].ToString());
            bool circ = int.Parse(json[i.ToString()]["circulation"].ToString()) != 0;
            Color rColour = Random.ColorHSV(0f, 1f, 0.4f, 0.7f, 0.5f, 1f);
            Color rColourO = Random.ColorHSV(0f, 1f, 0.4f, 0.7f, 0.5f, 1f);
            InstantiateButton(rName, 100f, -50-i*35, minSize, rColour, rColourO, circ);
        }

    }
    


    private void InstantiateButton(string rName,  float posX, float posY, float minSize, Color rColour, Color rColourO, bool circ)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);

        button.GetComponentInChildren<Text>().text = rName;

        // if the size was to be altered
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeH);
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeV);

        // add the create room function
        button.GetComponent<Button>().onClick.AddListener(() => CallCreateRoom(rName,  minSize, rColour, rColourO, circ));
        
    }

    private void CallCreateRoom(string rName,  float minSize, Color rColour, Color rColourO, bool circ) 
    {
        
        // create room
        movementController.CreateRoom(rName, minSize, rColour, rColourO, circ);

    }

    public void EnableInputField() 
    {
        // enable background image
        mainCanvas.GetComponent<Image>().enabled = true;

        //enable input field
        mainCanvas.GetComponentInChildren<InputField>(true).gameObject.SetActive(true);

    }

    public IEnumerator DisplayWarning(float duration) 
    {
        // enable background image
        mainCanvas.GetComponent<Image>().enabled = true;

        // enable warning screen
        errorScreen.GetComponentInChildren<Text>().text = "Room can be created only from tiles that have at least one common edge.";
        errorScreen.SetActive(true);
        yield return new WaitForSeconds(duration);
        errorScreen.SetActive(false);
        mainCanvas.GetComponent<Image>().enabled = false;

    }


    
    ///<remarks>
    ///NW
    ///[MenuItem("AssetDatabase/LoadAllAssetsAtPath")]
    ///If we want to instantiate anything from the project folder that is not in the game use this method, just create a resources folder with the prefabs
    ///</remarks>

}
