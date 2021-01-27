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

    #endregion

    #region Public Variables
    public Canvas mainCanvas;
    public GameObject buttonPrefab;
    public GameObject errorScreen;

    #endregion
    void Awake() 
    {
        movementController = gameObject.GetComponent<MovementController>();
        raycastController = gameObject.GetComponent<RaycastController>();
        SpawnButtons();
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
            Color rColour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            Color rColourO = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            InstantiateButton(rName, 100f, -50-i*35, minSize, rColour, rColourO);
        }

    }

    private void InstantiateButton(string rName,  float posX, float posY, float minSize, Color rColour, Color rColourO)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.GetComponent<RectTransform>().SetParent(mainCanvas.transform, false);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);

        button.GetComponentInChildren<Text>().text = rName;

        // if the size was to be altered
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeH);
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeV);

        // add the create room function
        button.GetComponent<Button>().onClick.AddListener(() => CallCreateRoom(rName,  minSize, rColour, rColourO));

    }

    private void CallCreateRoom(string rName,  float minSize, Color rColour, Color rColourO) 
    {
        
        // create room
        movementController.CreateRoom(rName, minSize, rColour, rColourO);

    }

    public void EnableInputField() 
    {
        // enable background image
        mainCanvas.GetComponent<Image>().enabled = true;

        //enable input field
        mainCanvas.GetComponentInChildren<InputField>(true).gameObject.SetActive(true);

    }

    public IEnumerator DisplayWarning(float duration, string rType, float minSize) 
    {
        // enable background image
        mainCanvas.GetComponent<Image>().enabled = true;

        // enable warning screen
        errorScreen.GetComponentInChildren<Text>().text = "The area of the room you are trying to create is lower than the minimum allowed for the "+rType+" type: "+minSize.ToString()+"m2";
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
