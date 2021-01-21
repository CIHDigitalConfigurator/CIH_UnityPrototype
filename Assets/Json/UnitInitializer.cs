using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInitializer : MonoBehaviour
{
    #region Public Variables
    List<EG_unit> units;
    #endregion

    #region Private Variables
    JObject unitJson;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndExectute(this.GetComponent<Reader>().jsonCount));
    }

    IEnumerator WaitAndExectute(int jsonCount)
    {
        yield return new WaitUntil(() => jsonCount > 0 );

        // Read from json list
        unitJson = this.GetComponent<Reader>().jsons[1];
        JArray unitsArray = JArray.Parse(unitJson["data"].ToString());

        Debug.Log("elo");
        // Build game object based on information stored in json
        units = new List<EG_unit>();
        for (int i = 0; i < unitsArray.Count; i++)
        {
            var unitParameters = unitsArray[i];

            // create new object
            GameObject unit = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var eg_unit = unit.AddComponent<EG_unit>();

            eg_unit.om_unit.CentrePoint = new Vector3
                    (unitParameters["centre_point"]["x"].ToObject<float>(),
                    unitParameters["centre_point"]["y"].ToObject<float>(),
                    unitParameters["centre_point"]["z"].ToObject<float>()
                    );
            eg_unit.om_unit.Width = unitParameters["width"].ToObject<float>();
            eg_unit.om_unit.Height = unitParameters["height"].ToObject<float>();
            eg_unit.om_unit.Rotation = unitParameters["rotation"].ToObject<float>();

        }
    }

}
