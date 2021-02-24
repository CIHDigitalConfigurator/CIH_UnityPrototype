using OM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Embedding the main gameflow logics
/// 
/// IMPORTANT THINGS TO NOTE:
/// - currently reading the names of speckle streams - this need to remain unchanged in order to preserve the tile tagging (not applicable while reading from json)
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
    private Validation validation;
    private int fCounter;
    #endregion

    void Awake()
    {
        effectsManager = gameObject.GetComponent<EffectsManager>();
        raycastController = gameObject.GetComponent<RaycastController>();
        UIController = gameObject.GetComponent<UIController>();
        validation = gameObject.GetComponent<Validation>();

        parentRoom = new GameObject
        {
            name = "ROOM"
        };
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
                MovBlocked = false;
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


    #region Functions triggered by runtime events

    /// <param name="rType"> room type comes with a set of parameters defining creation criteria; currently embedded in a json </param>
    public void CreateRoom(string rType, float minSize, Color rColour, Color rColourO, bool circ)
    {
        // check whether there are no disjointed segments

        if (AllMeshesHasCommonEdges(selectedObjects))
        {
            GameObject tempRoom = MergeMesh();
            float rArea = CalculateSurfaceArea(tempRoom.GetComponent<MeshFilter>().sharedMesh);
            currentArea = rArea;

            // find closest level and its height
            var height = tempRoom.GetComponent<MeshFilter>().sharedMesh.bounds.center.y;
            Tuple<float, float> levelParams = FindAssociatedHeightAndLevel(height);

            var vertices2D = Outline2DArray(tempRoom.GetComponent<MeshFilter>().mesh);

            validation.AddTextToValidation(rType, "name", minSize, rArea);

            // create room
            CurrentRoom = tempRoom;
            CurrentRoom.tag = "room";
            CurrentRoom.AddComponent<MeshRenderer>();
            CurrentRoom.AddComponent<MeshCollider>();
            CurrentRoom.GetComponent<Renderer>().material = matTemplate;
            CurrentRoom.GetComponent<Renderer>().material.SetColor("_Color", rColour);
            CurrentRoom.GetComponent<Renderer>().material.SetColor("_FirstOutlineColor", rColourO);


            CurrentRoom.AddComponent<EG_room>();
            CurrentRoom.GetComponent<EG_room>().Type = rType;
            CurrentRoom.GetComponent<EG_room>().Area = rArea;
            CurrentRoom.GetComponent<EG_room>().Vertices = vertices2D;
            CurrentRoom.GetComponent<EG_room>().Level = levelParams.Item1;
            CurrentRoom.GetComponent<EG_room>().Height = levelParams.Item2;
            CurrentRoom.GetComponent<EG_room>().Edges = new List<bool>();
            CurrentRoom.GetComponent<EG_room>().Circulation = circ;


            CurrentRoom.transform.SetParent(parentRoom.transform);

            UIController.EnableInputField();
        }
        else
        {
            StartCoroutine(UIController.DisplayWarning(3f));
            foreach (GameObject obj in selectedObjects)
            {
                obj.SetActive(true);
            }
        }

        Deselect();
    }


    public void DeleteRoom()
    {
        List<GameObject> roomList = new List<GameObject>();

        foreach (var room in selectedObjects)
        {
            roomList.Add(room);   
            Transform[] allChildren = room.GetComponentsInChildren<Transform>();
            List<GameObject> tempChildren = new List<GameObject>();
            for (int i = 0; i<room.transform.childCount; i++)
            {
                if (room.transform.GetChild(i).gameObject.tag == "tile")
                {
                    tempChildren.Add(room.transform.GetChild(i).gameObject);
                    room.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            foreach (var child in tempChildren) { child.transform.SetParent(GameObject.FindGameObjectWithTag("tileParent").transform); }
        }

        Deselect();

        foreach (var room in roomList) { Destroy(room); };
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
        CurrentRoom.GetComponent<EG_room>().Name = rname;


        nameText.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public static bool AllMeshesHasCommonEdges(List<GameObject> objects)
    {
        bool pass = true;
        if (objects.Count > 1)
        {
            List<bool> commonEdges = new List<bool>(objects.Count);

            for (int i = 0; i < objects.Count; i++)
            {
                bool commonEdgeForThisMesh = false;

                for (int j = 0; j < objects.Count; j++)
                {
                    if (i != j && EdgeHelper.MeshesHaveCommondge(objects[i], objects[j]) == true)
                    {
                        commonEdgeForThisMesh = true;
                    }
                }
                commonEdges.Add(commonEdgeForThisMesh);
            }

            pass = !commonEdges.Contains(false);
        }

        return pass;
    }

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

    public void EdgeTypesWriter() 
    {
        EdgeTypes();
    }
    #endregion

    #region Output utilities
    ///<summary>
    ///Finding the outline of a 2D mesh
    ///</summary>
    private static List<Vector2> Outline2DArray(Mesh mesh)
    {
        List<Vector2> vr = new List<Vector2>();
        var boundaryPath = EdgeHelper.GetEdges(mesh.triangles, mesh.vertices).FindBoundary().SortEdges();
        //Debug.Log("boundary path count " +boundaryPath.Count());
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < boundaryPath.Count; i++)
        {
            Vector3 pos = vertices[boundaryPath[i].v1];
            vr.Add(new Vector2(pos.x, pos.z));
            //Debug.Log(vertices[boundaryPath[i].v1]);
        }
        //Debug.Log("vector2d count " + vr.Count());
        return vr;
    }

    Tuple<float, float> FindAssociatedHeightAndLevel(float meshHeight)
    {
        OM_level closestLevel = gameObject.GetComponentInParent<VisibilityController>().FindClosestLevel(meshHeight);
        return Tuple.Create(closestLevel.Elevation, closestLevel.Height);
    }

    /// <summary>
    /// Assign the edge types to the rooms
    /// </summary>
    private static void EdgeTypes()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("room");

        // external edge -> True

        for (int i = 0; i < rooms.Length; i++) 
        { 
            rooms[i].GetComponent<EG_room>().BoundaryEdges = EdgeHelper.GetEdges(rooms[i].GetComponent<MeshFilter>().mesh.triangles, rooms[i].GetComponent<MeshFilter>().mesh.vertices).FindBoundary().SortEdges();
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            List<bool> edgeTypes_i = new List<bool>();
            foreach (var edge_i in rooms[i].GetComponent<EG_room>().BoundaryEdges)
            {
                bool intrnl = false;
                for (int j = 0; j < rooms.Length; j++)
                {
                    if (j != i)
                    {

                        foreach (var edge_j in rooms[j].GetComponent<EG_room>().BoundaryEdges)
                        {
                          
                            if (edge_i.v1x == edge_j.v1x && edge_i.v1z == edge_j.v1z && edge_i.v2x == edge_j.v2x && edge_i.v2z == edge_j.v2z|| (edge_i.v1x == edge_j.v2x && edge_i.v1z == edge_j.v2z && edge_i.v2x == edge_j.v1x && edge_i.v2z == edge_j.v1z))
                            {
                                intrnl = true;
                                break;
                            }
                        }
                    }
                }
                //Debug.Log("intrnl " + intrnl);
                edgeTypes_i.Add(!intrnl);
            }

            rooms[i].GetComponent<EG_room>().Edges = edgeTypes_i;

        }

    }

    #endregion


    #region Utilities
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
            selectedObjects[i].transform.SetParent(room.transform);
            selectedObjects[i].SetActive(false);
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        room.AddComponent<MeshFilter>();
        room.GetComponent<MeshFilter>().mesh = new Mesh();
        room.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
        room.transform.GetComponent<MeshFilter>().mesh.Optimize();
        room.transform.gameObject.SetActive(true);

        return room;
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


    private void FindChildren(string parent, string tg)
    {
        GameObject rt = GameObject.Find(parent);
        if (rt != null)
        {
            foreach (Transform child in rt.GetComponent<Transform>())
            {
                child.tag = tg;
                if (child.gameObject.GetComponent<MeshCollider>() == null) child.gameObject.AddComponent<MeshCollider>();

            }
        }
    }
    #endregion

    


}
