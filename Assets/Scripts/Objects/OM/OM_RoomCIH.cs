using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BH.oM.Base;
using BH.oM.Geometry;

public class RoomCIH : MonoBehaviour, IObject
{
    public string TypologyName { get; set; }
    public float MinArea { get; set; }
    public bool IsCirculation { get; set; }
    public float DaylightFactor { get; set; }
    public int AcousticSeparation { get; set; }
    
    /// <remarks>
    /// The following properties are associated with the 
    /// individual rooms generated in Module 2
    /// Polyline can be replaced with list of tuples
    /// </remarks>
    public string RoomName { get; set; }
    public Polyline Outline { get; set; }
    public List<bool> EdgesExternal { get; set; }
    public float Height { get; set; }
    public int Level { get; set; }

}
