using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public GameObject jsonReader;
    List<GameObject> parents;
    List<OM.OM_level> levels;

    // Start is called before the first frame update
    void Start()
    {
        // Wait few seconds for objects to initialize from json
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2);

        // Find all parent GameObject names
        var keys = jsonReader.GetComponent<Reader>().jsonFolder.Keys.ToArray();
        parents = FindParents(keys);

        // Find all levels
        levels = jsonReader.GetComponent<LayerInitializer>().levels;

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
        Tuple<float,float> leveRange = FindLevelRanges(levels, layerName);
        LoopThroughChildrenAndToggleVisibilityInRange(parents, leveRange.Item1, leveRange.Item2, false);

    }

    public void LevelShow(string layerName)
    {
        Tuple<float, float> leveRange = FindLevelRanges(levels, layerName);
        LoopThroughChildrenAndToggleVisibilityInRange(parents, leveRange.Item1, leveRange.Item2, true);
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
