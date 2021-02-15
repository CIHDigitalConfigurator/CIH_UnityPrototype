using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
        //private bool raycastBlock = false;
    private RaycastController raycastController;

        void Start() 
        {
        raycastController = GameObject.FindGameObjectWithTag("eventSystem").GetComponent<RaycastController>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            raycastController.RayBlocked = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            raycastController.RayBlocked =  false;
        }
 }
