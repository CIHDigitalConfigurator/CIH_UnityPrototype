using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

public class Writer : MonoBehaviour
{
    string relativeJsonpath = @"\Mott MacDonald\Platform Design Programme - 0.10 Digital Configurator\WP5 Reference Implementation\5.7 Implementation\02_Design Configurators\Objects\";
    string homePath;
    

    public void WriteJson()
    {
        homePath = Environment.GetEnvironmentVariable("HOMEPATH");

        var allRooms = GameObject.FindGameObjectsWithTag("room");

        List<EG_room> egrooms = new List<EG_room>();
        List<Dictionary<string, object>> all = new List<Dictionary<string, object>>();

        for (int i = 0; i < allRooms.Length; i++)
        {

            var eg_rooms = allRooms[i].GetComponent<EG_room>();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("name", eg_rooms.name);
            properties.Add("area", eg_rooms.Area);


            List<float[]> verts = new List<float[]>();

            for (int j = 0; j < eg_rooms.Vertices.Count; j++)
            {
                float[] vertices = new float[3];
                vertices[0] = eg_rooms.Vertices[j].x;
                vertices[0] = eg_rooms.Vertices[j].z;
                vertices[0] = eg_rooms.Vertices[j].y;

                verts.Add(vertices);
            }

            properties.Add("vertices", verts.ToArray());
            properties.Add("triangles", eg_rooms.Triangles);

            all.Add(properties);

            egrooms.Add(allRooms[i].GetComponent<EG_room>());
        }

        string aa = JsonConvert.SerializeObject(all.ToArray());
        

        System.IO.File.WriteAllText(homePath + relativeJsonpath + "out_Rooms.json", aa);

    }
}
