using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SV_test : MonoBehaviour
{
    /// <summary>
    /// This is not integrated into the current flow - just here to show that there can be a separate set of scripts for solving etc
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            GameObject[] allWalls;
            allWalls = GameObject.FindGameObjectsWithTag("wall");

            if (allWalls != null)
            {
                foreach (GameObject wall in allWalls)
                {
                    CollisionTest collisionCheck =  wall.AddComponent<CollisionTest>();
                }
            }
        }

    }

}
