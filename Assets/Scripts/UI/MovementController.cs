using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Embedding the main gameflow logics
/// 
/// IMPORTANT THINGS TO NOTE:
/// - currently reading the names of speckle streams - this need to remain unchanged in order to preserve the tile tagging (not applicable while reading from json)
/// 
/// TODO:
/// - need to add adjacency check (currently checking only for area)
/// 
/// </summary>
[RequireComponent(typeof(EffectsManager))]
public class MovementController : MonoBehaviour
{


    #region Public Variables
    public bool MovBlocked { get; set; } = true;
    public GameObject CurrentRoom { get; set; }
    public GameObject CurrentObject { get; set; }
    public List<GameObject> selectedObjects;
    
    public Material matTemplate;

    public Material site;

    public Text inputText;

    public float currentArea;

    GameObject parentRoom;

    #endregion

    #region Private Variables
    
    private EffectsManager effectsManager;
    private RaycastController raycastController;
    private UIController UIController;
    private int fCounter;
    #endregion

    void Awake()
    {
        effectsManager = gameObject.GetComponent<EffectsManager>();
        raycastController = gameObject.GetComponent<RaycastController>();
        UIController = gameObject.GetComponent<UIController>();

        parentRoom = new GameObject();
        parentRoom.name = "ROOM";
    }


    void Update()
    {

        ///<remarks>
        ///Selecting the tiles
        ///</remarks>

        if (Input.GetMouseButtonUp(0))
        {

            GameObject tempObject = raycastController.OnClick();

            if (tempObject != null)
            {
                CurrentObject = tempObject;
                MovBlocked  = false;
                //effectsManager.ChangeCurrentObject(currentObject);
                if (!selectedObjects.Contains(tempObject))
                {
                    selectedObjects.Add(tempObject);
                }
                else 
                {
                    selectedObjects.Remove(tempObject);
                }
                effectsManager.UpdateEmission(selectedObjects);
            }


        }

        ///<remarks>
        ///Start from updating the tags and materials on the speckle stream objects every 1000 frames
        ///</remarks>

        fCounter++;

        if (fCounter > 1000) 
        {
            FindChildren("BH_grid_tiles", "tile");

            // change material for the site objects

            GameObject rt = GameObject.Find("Obj_site");
            if (rt != null)
            {
                foreach (Transform child in rt.GetComponent<Transform>())
                {
                    if (child.GetComponent<Renderer>().material != site) child.GetComponent<Renderer>().material = site;

                }
            }

            fCounter = 0;
        }

    }


    #region Utilities

    ///<remarks>
    ///Used to deselect the tiles and turn the rendering back to normal - also accesible via the UI button
    ///</remarks>
    public void Deselect() 
    {
        MovBlocked = true;
        CurrentObject = null;
        selectedObjects.Clear();
        effectsManager.UpdateEmission(selectedObjects);
    }



    private void FindChildren(string parent, string tg) 
    {
        GameObject rt = GameObject.Find(parent);
        if (rt != null) 
        {
            foreach (Transform child in rt.GetComponent<Transform>()) 
            {
                child.tag = tg;
                if (child.gameObject.GetComponent<MeshCollider>()==null) child.gameObject.AddComponent<MeshCollider>();
                //if( child.GetComponent<Renderer>().material != tile) child.GetComponent<Renderer>().material = tile;

            }
        }
    }
    /// <summary>
    ///  creating a combined mesh for a room layout 
    /// </summary>
    private GameObject MergeMesh() 
    {
        GameObject room = new GameObject();
        
        MeshFilter[] meshFilters = new MeshFilter[selectedObjects.Count];
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            meshFilters[i] = selectedObjects[i].GetComponent<MeshFilter>();
            selectedObjects[i].SetActive(false);
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        room.AddComponent<MeshFilter>();
        room.GetComponent<MeshFilter>().mesh = new Mesh();
        room.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        room.transform.gameObject.SetActive(true);
        return room;
    }


    #endregion
    

    /// <param name="rType"> room type comes with a set of parameters defining creation criteria; currently embedded in a json </param>
    public void CreateRoom(string rType, float minSize, Color rColour, Color rColourO)
    {
        GameObject tempRoom = MergeMesh();
        float rArea = CalculateSurfaceArea(tempRoom.GetComponent<MeshFilter>().sharedMesh);
        currentArea = rArea;


        var vertices = tempRoom.GetComponent<MeshFilter>().sharedMesh.vertices.ToList();
        var triangles = tempRoom.GetComponent<MeshFilter>().sharedMesh.triangles.ToList();


        if (rArea >= minSize)
        {
            CurrentRoom = tempRoom;
            CurrentRoom.tag = "room";
            CurrentRoom.AddComponent<MeshRenderer>();
            CurrentRoom.GetComponent<Renderer>().material = matTemplate;
            CurrentRoom.GetComponent<Renderer>().material.SetColor("_Color", rColour);
            CurrentRoom.GetComponent<Renderer>().material.SetColor("_FirstOutlineColor", rColourO);

            CurrentRoom.AddComponent<EG_room>();
            CurrentRoom.GetComponent<EG_room>().Name = rType;
            CurrentRoom.GetComponent<EG_room>().Area = rArea;
            CurrentRoom.GetComponent<EG_room>().MinArea = minSize;
            CurrentRoom.GetComponent<EG_room>().Vertices = vertices;
            CurrentRoom.GetComponent<EG_room>().Triangles = triangles;

            CurrentRoom.transform.SetParent(parentRoom.transform);

            UIController.EnableInputField();

        } else
        {
            StartCoroutine(UIController.DisplayWarning(5f, rType, minSize));
            foreach (GameObject obj in selectedObjects)
            {
                obj.SetActive(true);
            }
            

        }

        Deselect();
    }

    /// <summary>
    /// adding the  name as text to the displayed room mesh
    /// this is where we would also initialise serialisation (to pass to the next configurator)
    /// </summary>
    public void NameRoom() 
    {
        var tiles = GameObject.FindGameObjectsWithTag("tile");

        string rname = inputText.text;
        CurrentRoom.name = rname;
        GameObject nameText = new GameObject();
        nameText.AddComponent<TextMesh>();
        nameText.GetComponent<TextMesh>().text = rname + "\n " + (Math.Round(currentArea)).ToString() + " m²";
        nameText.transform.position = CurrentRoom.GetComponent<Renderer>().bounds.center;
        nameText.transform.parent = CurrentRoom.transform;
        nameText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
        nameText.GetComponent<TextMesh>().anchor = TextAnchor.UpperCenter;
        nameText.GetComponent<TextMesh>().characterSize = 0.1f;
        nameText.GetComponent<TextMesh>().fontSize = 100;
        
        nameText.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }



    ///<summary>
    ///For area check
    ///</summary>
    private static float CalculateSurfaceArea(Mesh mesh)
    {

        var triangles = mesh.triangles;
        var vertices = mesh.vertices;

        double sum = 0.0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;

            sum += Vector3.Cross(a, b).magnitude;
        }

        return (float)(sum / 2.0);
    }


    /*
        #region Keyboard Navigation
        /// <summary>
        ///  Keyboard navigation - currently not in use
        ///  requires the following vars
        ///  
    public float LerpTimeTranslation  = 0.4f;
    public float LerpTimeRotation = 0.2f;
    public float UnitDistance = 1f;
    public float UnitRotationAngle= 7.5f;

    private KeyCode[] movementKeys = { KeyCode.W, KeyCode.S, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    private KeyCode[] otherKeys = { KeyCode.RightShift, KeyCode.LeftShift };
    
    /// </summary>
    private void KeyboardNav()
        {
            // Using keyboard for linear movement
            if (Input.anyKey && !movBlocked)
            {

                bool rot = true ? Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) : false;
                float lerpTime = rot ? LerpTimeRotation : LerpTimeTranslation;
                var unitP = new Vector3();
                var unitR = new Quaternion();
                var unitRInc = UnitRotationAngle;

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    unitP = UnitDistance * Vector3.back;
                    unitR = Quaternion.Euler(-unitRInc, 0, 0);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    unitP = UnitDistance * Vector3.forward;
                    unitR = Quaternion.Euler(unitRInc, 0, 0);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    unitP = UnitDistance * Vector3.left;
                    unitR = Quaternion.Euler(0, unitRInc, 0);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    unitP = UnitDistance * Vector3.right;
                    unitR = Quaternion.Euler(0, -unitRInc, 0);
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    unitP = UnitDistance * Vector3.up;
                    unitR = Quaternion.Euler(0, 0, unitRInc);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    unitP = UnitDistance * Vector3.down;
                    unitR = Quaternion.Euler(0, 0, -unitRInc);
                }

                foreach (var k in movementKeys)
                {
                    if (Input.GetKeyDown(k))
                        StartCoroutine(moveOrRotateByUnit(currentObject.transform, unitP, unitR, lerpTime, rot));
                }

            }
        }

        private IEnumerator moveOrRotateByUnit(Transform objectToMove, Vector3 unitVector, Quaternion unitQuaternion, float lerpTime, bool rot)
        {
            movBlocked = true;
            var startPosition = objectToMove.localPosition;
            var startRotation = objectToMove.localRotation;
            var startTime = Time.time;

            while (Time.time - startTime < lerpTime)
            {
                if (rot)
                {
                    var updatedRotation = Quaternion.Lerp(startRotation, startRotation * unitQuaternion, (Time.time - startTime) / lerpTime);
                    objectToMove.localRotation = updatedRotation;
                }
                else
                {
                    var updatedPosition = Vector3.Lerp(startPosition, unitVector + startPosition, (Time.time - startTime) / lerpTime);
                    objectToMove.localPosition = updatedPosition;
                }
                yield return null;
            }

            //  option is to use just this instead too, if no sooth transition preferred
            //objectToMove.localPosition = unitVector + startPosition;
            movBlocked = false;
            yield return null;
        }

        #endregion
        */
}
