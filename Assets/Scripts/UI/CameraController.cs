using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour

{
    #region Public Variables

    public Camera camera2D;

    public float SensitivityRotation = 2f;
    public float SensitivityPosition = 2f;
    public float SensitivityScroll = 2f;

    
    #endregion
    #region Private Variables
    //private float maxVertAngle = 80f;
    private Vector2 camPosition;
    private Vector2 camRotation;
    private float fov, prevWheel, size;

    private bool c2D;
    #endregion
    void Start() 
    {

        camPosition.x = Camera.main.transform.position.x;
        camPosition.y = Camera.main.transform.position.y;

        camRotation.y = Camera.main.transform.rotation.eulerAngles.x;
        camRotation.x = Camera.main.transform.rotation.eulerAngles.y;

        fov = Camera.main.fieldOfView;
        size = camera2D.orthographicSize;

        prevWheel = Input.GetAxis("Mouse ScrollWheel");

        
    }
    
    void Update()
    {
        ///<summary>
        ///Camera rotation update
        /// </summary>
        
        if (Input.GetMouseButton(1)) 
        {
            camRotation.x += Input.GetAxis("Mouse X") * SensitivityRotation;
            camRotation.y -= Input.GetAxis("Mouse Y") * SensitivityRotation;
            camRotation.x = Mathf.Repeat(camRotation.x, 360);
            // clamping vertical rotation - can be added back if required
            //currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);  
            Camera.main.transform.rotation = Quaternion.Euler(camRotation.y, camRotation.x, Camera.main.transform.rotation.eulerAngles.z);
        }

        if (Input.GetMouseButton(2)) 
        {
            camPosition.x -= Input.GetAxis("Mouse X") * SensitivityPosition;
            camPosition.y -= Input.GetAxis("Mouse Y") * SensitivityPosition;
            Camera.main.transform.position = new Vector3(camPosition.x, camPosition.y, Camera.main.transform.position.z);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != prevWheel) 
        {
            
            if (c2D)
            {
                fov -= Input.GetAxis("Mouse ScrollWheel") * SensitivityScroll;
                Camera.main.fieldOfView = fov;
            }
            else 
            {
                size -= Input.GetAxis("Mouse ScrollWheel") * SensitivityScroll;
                camera2D.orthographicSize = size;
            }
            
        }

    }



    public void Toggle2D3D(bool isOn)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = !isOn;
        camera2D.enabled = isOn;
    }

}