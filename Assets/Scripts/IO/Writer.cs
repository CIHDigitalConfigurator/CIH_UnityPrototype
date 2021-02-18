using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using UnityEngine;

public class Writer : MonoBehaviour
{
    readonly string relativeJsonpath = @"\Mott MacDonald\Platform Design Programme - 0.10 Digital Configurator\WP5 Reference Implementation\5.7 Implementation\02_Design Configurators\Objects\";
    string homePath;
    List<Dictionary<string, object>> roomData;
    string serializedRoomData;

    public void WriteJson()
    {
        serializedRoomData = " ";
        //roomData.Clear();

        homePath = Environment.GetEnvironmentVariable("HOMEPATH");
        GameObject[] allRooms = GameObject.FindGameObjectsWithTag("room");

        roomData = new List<Dictionary<string, object>>();


        for (int i = 0; i < allRooms.Length; i++)
        {

            EG_room eg_room = allRooms[i].GetComponent<EG_room>();
            
            // convert Vector2 to arrays
            double[][] roomsVertices = eg_room.Vertices.Select(v => new double[2] { Math.Round(v.x, 2), Math.Round(v.y, 2) }).ToArray();

            Dictionary<string, object> roomProperties = new Dictionary<string, object>()
            {
                { "name", eg_room.Name },
                { "type", eg_room.Type },
                { "vertices", roomsVertices },
                { "height", eg_room.Height },
                { "level", eg_room.Level },
                { "edgesExternal", eg_room.Edges },
                { "circulation", eg_room.Circulation }
            };

            roomData.Add(roomProperties);

        }

        serializedRoomData = JsonConvert.SerializeObject(roomData.ToArray());

        string filePath = homePath + relativeJsonpath + "02_OUT_Rooms.json";

        if (!File.Exists(filePath))
           System.IO.File.WriteAllText(filePath, serializedRoomData);
        else
        {
            File.Delete(filePath);
            File.WriteAllText(filePath, serializedRoomData);
        }

       // System.IO.File.WriteAllText(filePath, serializedRoomData);
    }
}
