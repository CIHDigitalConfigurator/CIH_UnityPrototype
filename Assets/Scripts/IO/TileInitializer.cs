using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileInitializer : MonoBehaviour
{
    #region Public Variables
    public Material material;

    #endregion

    #region Private Variables
    GameObject parentTile;
    List<GameObject> childrenTiles;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitUntilJsonsAndExecute());
    }

    IEnumerator WaitUntilJsonsAndExecute()
    {
        yield return new WaitUntil(() => this.GetComponent<LayerInitializer>().layersCompleted);

        // Get tiles values from dictionary to array
        JArray tileArray = JArray.Parse(GetComponent<Reader>().jsonFolder["02_IN_Tiles"]["tile"].ToString());

        // create parent gameobject
        parentTile = new GameObject
        {
            name = "TILE"
        };

        // Build game object based on information stored in json
        childrenTiles = new List<GameObject>();

        foreach (JToken tileData in tileArray)
        {
            childrenTiles.Add(InstatiateTile(tileData, parentTile, material));
        }
    }

    GameObject InstatiateTile(JToken parameters, GameObject parent, Material m)
    {
        // create new object
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tile.transform.SetParent(parent.transform, false);

        tile.GetComponent<MeshRenderer>().material = m;
        tile.layer = LayerMask.NameToLayer("Level " + parameters["level"].ToObject<string>());

        // child just for preview
        //GameObject childMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //childMesh.name = "preview mesh";
        //childMesh.transform.SetParent(tile.transform);


        // attach eg object
        var eg_tile = tile.AddComponent<EG_tile>();

        Vector3 centrePoint = new Vector3
                (parameters["centre_point"]["x"].ToObject<float>(),
                 parameters["centre_point"]["z"].ToObject<float>(),
                 parameters["centre_point"]["y"].ToObject<float>()
    );

        // assign parameters
        eg_tile.om_tile.CentrePoint = centrePoint;
        eg_tile.om_tile.Width = parameters["width"].ToObject<float>();
        eg_tile.om_tile.Depth = parameters["depth"].ToObject<float>();
        eg_tile.om_tile.Height = parameters["height"].ToObject<float>();
        eg_tile.om_tile.Rotation = -parameters["rotation"].ToObject<float>();
        eg_tile.om_tile.Level = parameters["level"].ToObject<int>();

        //childMesh.GetComponent<MeshRenderer>().material = m;


        return tile;
    }


}
