using OM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OM.OM_wall;
using static OM.OM_utils;
using System.IO;
using Newtonsoft.Json.Linq;

namespace EG
{

    public class EG_wall : MonoBehaviour
    {
        OM_wall om_wall = new OM_wall();
        float collider_height_percentage = 0.2f;

        void Start()

        {
            //read json
            string path = "Assets/Json/set_data.json";
            StreamReader a = new StreamReader(path);
            JObject json = JObject.Parse(a.ReadToEnd());

            // read dimensions from json to script
            om_wall.Width = json["wall"]["width"].Value<float>();
            om_wall.Depth = json["wall"]["depth"].Value<float>();
            om_wall.Height = json["wall"]["height"].Value<float>();

            a.Close();

            // find offset of wall base based on floor height
            var wall_base_offset = json["slab"]["height"].Value<float>() / 2;

            // move wall up and scale to json data
            this.transform.localScale = new Vector3((float)om_wall.Width, (float)om_wall.Height, (float)om_wall.Depth);
            this.transform.position = new Vector3(this.transform.position.x, (float)om_wall.Height/2 + wall_base_offset, this.transform.position.z);


            // add allowed collider objects
            om_wall.allowed_colliders = new List<string>() { "slab" };

            // add extra child collider in lower part of wall
            var child_slab_collider = AddChildCollider("slab_collider", collider_height_percentage, wall_base_offset);

        }

        // This function adds extra collider to a lower part of wall. This collider can be used for example for slab detection. 
        public GameObject AddChildCollider (string collider_name, float collider_height_percentage, float wall_offset)
        {
            var wall_collider = this.GetComponent<BoxCollider>();
            GameObject child = new GameObject(collider_name);

            var child_collider = child.AddComponent<BoxCollider>();
            child.transform.SetParent(this.transform);

            child.transform.localScale = new Vector3(1, 1, 1);
            child.transform.localPosition = new Vector3(0, 0, 0);

            child_collider.size = new Vector3(1, collider_height_percentage, 1);
            child_collider.center = new Vector3(0, wall_offset + collider_height_percentage*0.5f - collider_height_percentage * (float)om_wall.Height - 0.1f, 0);


            return child;
        }

    }
}
