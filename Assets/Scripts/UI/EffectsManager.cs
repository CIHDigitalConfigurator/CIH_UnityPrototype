using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IMPORTANT THINGS TO NOTE:
/// - the UpdateEmission method will remove the emission component of all selectable objects (and bring it back to those selected)
/// </summary>
public class EffectsManager : MonoBehaviour
{
    
    #region Private Variables

    RaycastController raycastController;
    #endregion
    void Awake()
    {
        raycastController = gameObject.GetComponent<RaycastController>();

    }
    public void UpdateEmission(System.Collections.Generic.List<UnityEngine.GameObject> selectedObjects) 
    {
        
        foreach (var tg in raycastController.TagList)
        {
            foreach (var go in GameObject.FindGameObjectsWithTag(tg)) 
            {
                go.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            }
        }
        foreach (var so in selectedObjects) 
        {
            ChangeEmission(so, Color.green, 0.1f);
        }
    }


    #region Internal Utilities



    private void ChangeEmission(GameObject gameObject, Color color, float emissionIntensity)
    {
        try
        {
            if (gameObject.GetComponent<Renderer>() != null)
            {
                gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", color*emissionIntensity);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("material issue: " + e);
        }
        
    }
    // not used atm
    private void LerpAlbedo(GameObject gameObject, Color colorStart, Color colorFinish, float period) 
    {

        try
        {
            if (gameObject != null && gameObject.GetComponent<Renderer>() != null)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorFinish, Mathf.Sin(Time.time / period));
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("material issue: " + e);
        }

    }


    #endregion
}
