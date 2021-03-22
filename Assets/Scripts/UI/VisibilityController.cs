using OM;
using SpeckleUnity;
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

    // Object that will be taken into account when controlling visibility from UI
    readonly string[] parentNames = new string[3] {"ROOM", "TILE", "EXTERIOR" };

    void Start()
    {
        // Wait few seconds for objects to initialize from json
        StartCoroutine(DelayStart());
    }

    public IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2);

        parents = FindParents(parentNames);
        levels = jsonReader.GetComponent<LevelInitializer>().levels;

    }


    public void LevelHide(string layerName)
    {
        parents = FindParents(parentNames);
        Tuple<float, float> leveRange = FindLevelRanges(levels, layerName);
        ToggleVisibilityForEachParentObject(parents, leveRange.Item1, leveRange.Item2, false);

    }

    public void LevelShow(string layerName)
    {
        parents = FindParents(parentNames);
        Tuple<float, float> leveRange = FindLevelRanges(levels, layerName);
        ToggleVisibilityForEachParentObject(parents, leveRange.Item1, leveRange.Item2, true);
    }


    // Find gameobjects by name
    List<GameObject> FindParents(string[] parentNames)
    {
        var parents = from name in parentNames
                      where GameObject.Find(name.ToUpper()) != null
                      select GameObject.Find(name.ToUpper());

        return parents.ToList();
    }

    // Find min and max y value for selected level
    Tuple<float, float> FindLevelRanges(List<OM.OM_level> lvls, string layerName)
    {
        var levelFromName = lvls.First(x => x.Name == layerName.Split(' ')[1]);

        var rangeStart = levelFromName.Elevation;
        var rangeEnd = rangeStart + levelFromName.Height;

        return Tuple.Create(rangeStart, rangeEnd);
    }


    // Turn on or off MeshRenderer and MeshCollider according to transform.position.y and given ranges of level to be toggles
    void ToggleVisibilityForEachParentObject(List<GameObject> parents, float min, float max, bool onOff)
    {
        foreach (GameObject parent in parents)
        {
            ToggleVisibilityForChildren(parent, min, max, onOff);
        }
    }

    void ToggleVisibilityForChildren(GameObject parent, float min, float max, bool onOff)
    {
        for (int i = 0; i < parent.gameObject.transform.childCount; i++)
        {
            Transform[] allChildren = parent.GetComponentsInChildren<Transform>();

            foreach (Transform child in allChildren)
            {
                if (child.gameObject.GetComponent<MeshRenderer>() != null)
                {
                    float meshCenter = child.gameObject.GetComponent<MeshRenderer>().bounds.center.y;
                    if (meshCenter >= min && meshCenter < max)
                    {
                        child.gameObject.GetComponent<MeshRenderer>().enabled = onOff;

                        if (child.gameObject.GetComponent<MeshCollider>() != null)
                        {
                            child.gameObject.GetComponent<MeshCollider>().enabled = onOff;
                        }
                    }
                }



            }
        }
    }

    // Find closest OM_Level according to given y value
    public OM_level FindClosestLevel(float number)
    {
        OM_level closest = levels.OrderBy(item => Math.Abs(number - item.Elevation)).First();
        return closest;
    }

}
