using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    //public Material[] material;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        
        //rend.sharedMaterial = material[0];
        
    }


    private void OnTriggerEnter(Collider collision)
    {
        string colliderTag = collision.tag;
        //rend = GetComponent<Renderer>();
        Debug.Log(tag);
        switch (colliderTag)
        {
            case "slab":
                Debug.Log("connected to slab");

                //rend.sharedMaterial = material[1];
                
                rend.material.SetColor("_Color", Color.blue);
                break;

            case "wall":
                Debug.Log("connected to wall");

                //rend.sharedMaterial = material[2];

                rend.material.SetColor("_Color", Color.green);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
