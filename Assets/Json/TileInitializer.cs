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
    JObject json;
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
        yield return new WaitUntil(JsonsAreLoaded);

        // Get tiles values from dictionary to array
        JArray tileArray = JArray.Parse(GetComponent<Reader>().jsonFolder["tile"]["tile"].ToString());

        // create parent gameobject
        parentTile = new GameObject();
        parentTile.name = "tiles";

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
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tile.transform.SetParent(parent.transform, false);
        tile.GetComponent<MeshRenderer>().material = m;

        // attach eg object
        var eg_tile = tile.AddComponent<EG_tile>();

        // assign parameters
        eg_tile.om_tile.CentrePoint = new Vector3
                (parameters["centre_point"]["x"].ToObject<float>(),
                 parameters["centre_point"]["z"].ToObject<float>(),
                 parameters["centre_point"]["y"].ToObject<float>()
    );
        eg_tile.om_tile.Width = parameters["width"].ToObject<float>();
        eg_tile.om_tile.Depth = parameters["depth"].ToObject<float>();
        eg_tile.om_tile.Height = 0.1f;//parameters["height"].ToObject<float>();
        eg_tile.om_tile.Rotation = -parameters["rotation"].ToObject<float>();
        eg_tile.om_tile.Level = parameters["level"].ToObject<int>();

        return tile;
    }

    bool JsonsAreLoaded()
    {
        if (this.GetComponent<Reader>().jsonCount == this.GetComponent<Reader>().fileNames.Count)
        {
            return true;
        }
        return false;
    }

}
