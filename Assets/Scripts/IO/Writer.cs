using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

public class Writer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void WriteJson()
    {
        string _data = "json test";
        string json = JsonConvert.SerializeObject(_data, Formatting.Indented);



        var allRooms = GameObject.FindGameObjectsWithTag("room");

        List<EG_room> egrooms = new List<EG_room>();

        List<Dictionary<string, object>> all = new List<Dictionary<string, object>>();

        for (int i = 0; i < allRooms.Length; i++)
        {

            var rr = allRooms[i].GetComponent<EG_room>();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("name", rr.name);
            properties.Add("area", rr.Area);


            List<float[]> verts = new List<float[]>();

            for (int j = 0; j < rr.Vertices.Count; j++)
            {
                //Dictionary<string, float> vertices = new Dictionary<string, float>();
                float[] vertices = new float[3];
                vertices[0] = rr.Vertices[j].x;
                vertices[0] = rr.Vertices[j].z;
                vertices[0] = rr.Vertices[j].y;

                verts.Add(vertices);
            }

            properties.Add("vertices", verts.ToArray());

            all.Add(properties);

            egrooms.Add(allRooms[i].GetComponent<EG_room>());
        }

        string aa = JsonConvert.SerializeObject(all.ToArray());


        //write string to file
        System.IO.File.WriteAllText(@"C:\Users\justyna.szychowska\OneDrive - Grimshaw Architects\Desktop\path.json", aa);
        //open file stream
        //using (StreamWriter file = File.CreateText(@"C:\Users\justyna.szychowska\OneDrive - Grimshaw Architects\Desktop\path.json"))
        //{
        //   JsonSerializer serializer = new JsonSerializer();
        //serialize object directly into file stream
        //   serializer.Serialize(file, aa);
        //}
    }
}
