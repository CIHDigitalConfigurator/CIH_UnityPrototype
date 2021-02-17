﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerVisibilityController : MonoBehaviour
{
    public GameObject layerReader;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WaitUntilLayers()
    {
        yield return new WaitUntil(() => layerReader.GetComponent<LayerInitializer>().layersCompleted);
        LayerCullingHide(Camera.main, "Level 3");
    }

    public void LayerCullingShow(Camera cam, int layerMask)
    {
        cam.cullingMask |= layerMask;
    }

    public void LayerCullingShow(Camera cam, string layer)
    {
        LayerCullingShow(cam, 1 << LayerMask.NameToLayer(layer));
    }

    public void LayerCullingHide(Camera cam, int layerMask)
    {
        cam.cullingMask &= ~layerMask;
    }

    public void LayerCullingHide(Camera cam, string layer)
    {
        LayerCullingHide(cam, 1 << LayerMask.NameToLayer(layer));
    }
}