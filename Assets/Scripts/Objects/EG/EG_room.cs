using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EG_room : MonoBehaviour
{
    public string Name { get; set; }
    
    public string Type { get; set; }
    public float Area { get; set; }
    public float Height { get; set; }
    public List<Vector2> Vertices { get; set; }
    public float Level { get; set; }

    public List<bool> Edges { get; set; }

    public bool Circulation { get; set; }

}
