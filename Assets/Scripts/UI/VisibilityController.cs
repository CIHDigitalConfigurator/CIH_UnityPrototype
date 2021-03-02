using OM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public GameObject jsonReader;
    List<GameObject> parents;
    public List<OM_level> levels;
    readonly string[] parentNames = new string[2] { "ROOM", "TILE" };

    // Start is called before the first frame update
    void Start()
    {
        // Wait few seconds for objects to initialize from json
        StartCoroutine(DelayStart());
    }

    public IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2);

 
        // Find all parent GameObject names
        //var keys = jsonReader.GetComponent<Reader>().jsonFolder.Keys.ToArray();

        parents = FindParents(parentNames);
        //parents = tempObjectList.SelectMany(d => d).ToList();

        // Find all levels
        levels = jsonReader.GetComponent<LayerInitializer>().levels;

    }

    public OM_level FindClosestLevel(float number)
    {
        OM_level closest = levels.OrderBy(item => Math.Abs(number - item.Elevation)).First();
        return closest;
    }

    List<GameObject> FindParents(string[] parentNames)
    {
        var parents = from name in parentNames
                      where GameObject.Find(name.ToUpper()) != null
                      select GameObject.Find(name.ToUpper());

        return parents.ToList();
    }


    public void LevelHide(string layerName)
    {
        /* parents = FindParents(parentNames);
         Tuple<float,float> leveRange = FindLevelRanges(levels, layerName);
         LoopThroughChildrenAndToggleVisibilityInRange(parents, leveRange.Item1, leveRange.Item2, false);*/
        ///TEMPORARY CODE
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {
            if (go.layer == LayerMask.NameToLayer(layerName))
            {
                go.GetComponent<MeshRenderer>().enabled = false;
                try
                {
                    go.GetComponent<MeshCollider>().enabled = false;
                }
                catch
                { }
            }
        }

    }

    public void LevelShow(string layerName)
    {
    
        ///TEMPORARY CODE
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {
            if (go.layer == LayerMask.NameToLayer(layerName))
            {
                go.GetComponent<MeshRenderer>().enabled = true;
                try
                {
                    go.GetComponent<MeshCollider>().enabled = true;
                }
                catch
                {

                }
            }
        }
        /*parents = FindParents(parentNames);
        print("Foreach loop: " + child);
        Tuple<float, float> leveRange = FindLevelRanges(levels, layerName);
        LoopThroughChildrenAndToggleVisibilityInRange(parents, leveRange.Item1, leveRange.Item2, true);*/
    }

    Tuple<float, float> FindLevelRanges(List<OM.OM_level> lvls, string layerName)
    {
        var levelFromName = lvls.First(x => x.Name == layerName.Split(' ')[1]);

        var rangeStart = levelFromName.Elevation;
        var rangeEnd = rangeStart + levelFromName.Height;

        return Tuple.Create(rangeStart, rangeEnd);
    }


    void LoopThroughChildrenAndToggleVisibilityInRange(List<GameObject> parents, float min, float max, bool On)
    {
        foreach (GameObject parent in parents)
        {
            ToggleChildrenVisibilityInRange(parent, min, max, On);
        }
    }

    void ToggleChildrenVisibilityInRange(GameObject parent, float min, float max, bool on)
    {
        for (int i = 0; i < parent.gameObject.transform.childCount; i++)
        {
            var height = parent.gameObject.transform.GetChild(i).transform.position.y;

            if (height >= min && height < max)
            {
                parent.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = on;
            }
        }
    }

}
