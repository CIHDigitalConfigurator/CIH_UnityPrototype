using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewsControler : MonoBehaviour
{
    public Camera camera2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Toggle2D3D(bool isOn)
    {
        if (isOn)
        {
            Camera.main.enabled = false;
            camera2D.enabled = true;
        }
        else
        {
            Camera.main.enabled = true;
            camera2D.enabled = false;
        }
    }
}
