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
    public string[] TagList = { "tile", "room" };
    // Start is called before the first frame update

    private MovementController movementController;
    void Awake()
    {
        movementController = gameObject.GetComponent<MovementController>();
    }

    public GameObject OnClick()
    {

        if (RayBlocked)
        {
            return null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                    else if (tg == "room") 
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
