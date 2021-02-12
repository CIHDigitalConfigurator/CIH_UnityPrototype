using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EG_room : MonoBehaviour
{
    public string Name { get; set; }
    public float MinArea { get; set; }
    public float Area { get; set; }
    public float Height { get; set; }
    public List<Vector3> Vertices { get; set; }
    public List<int> Triangles { get; set; }

}
