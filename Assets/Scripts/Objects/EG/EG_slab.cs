using Newtonsoft.Json.Linq;
using OM;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static OM.OM_slab;

public class EG_slab : MonoBehaviour
{

    OM_slab om_slab = new OM_slab();

    // Start is called before the first frame update
    void Start()
    {
        //read json
        string path = "Assets/Json/set_data.json";
        StreamReader a = new StreamReader(path);
        JObject json = JObject.Parse(a.ReadToEnd());

        // assing dimensions from prefab for now
        om_slab.Width = json["slab"]["width"].Value<float>();
        om_slab.Depth = json["slab"]["depth"].Value<float>();
        om_slab.Height = json["slab"]["height"].Value<float>();

        a.Close();

        this.transform.localScale = new Vector3((float)om_slab.Width, (float)om_slab.Height, (float)om_slab.Depth);

        // setup rotations
        om_slab.axis_x = OM_utils.Rotations.constrained;
        om_slab.axis_y = OM_utils.Rotations.any;
        om_slab.axis_z = OM_utils.Rotations.constrained;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
