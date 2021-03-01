using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public bool RayBlocked { get; set; }  = false;
    public bool clicked = false;

    /// <remarks>
    /// Tag list allows for specifying which objects are selectable in run time (via tags)
    /// ATM 'tile' is hardcoded as a tag for selectable voxals
    /// </remarks>
    public string[] TagList = { "tile", "room", "exterior" };
    // Start is called before the first frame update

    private MovementController movementController;
    private CameraController cameraContoller;
    void Awake()
    {
        movementController = gameObject.GetComponent<MovementController>();
        cameraContoller = Camera.main.gameObject.GetComponent<CameraController>();
    }

    public GameObject OnClick()
    {

        if (RayBlocked)
        {
            return null;
        }

        //Camera camera = (Camera)FindObjectOfType(typeof(Camera));
        Camera camera =  cameraContoller.camera2D.enabled ? cameraContoller.camera2D : Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var clickHit);
        GameObject retOb = null;

        if (clickHit.collider != null)
        {

            foreach (var tg in TagList)
            {
                if (clickHit.collider.gameObject.tag == tg)
                {
                    if (tg == "tile")
                    {
                        bool rInSel = false;
                        foreach (var objts in movementController.selectedObjects) 
                        {
                            if (objts.CompareTag("room"))
                            {
                                rInSel = true;
                                Debug.Log("room in sel rooms");
                            }
                        }
                        clicked = true;
                        if (!rInSel) retOb = clickHit.collider.gameObject;
                    }
                    else if (tg == "room" || tg == "exterior") 
                    {
                        movementController.Deselect();
                        clicked = true;
                        retOb = clickHit.collider.gameObject;
                    }

                }
            }
        }

        return retOb;

    }

}
