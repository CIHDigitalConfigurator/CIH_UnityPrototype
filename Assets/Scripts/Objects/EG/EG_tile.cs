﻿using OM;
using System.Collections;
using UnityEngine;

public class EG_tile : MonoBehaviour
{
    public OM_tile om_tile = new OM_tile();
 

    void Start()
    {
        StartCoroutine(WaitAndExectute());
    }

    IEnumerator WaitAndExectute()
    {
        yield return new WaitUntil(() => ParametersHaveValues());

        this.name = "tile";
        tag = "tile";
        transform.position = om_tile.CentrePoint.Value;
        transform.localScale = new Vector3(om_tile.Width.Value, 0.1f, om_tile.Depth.Value);
        transform.Rotate(0, om_tile.Rotation.Value, 0);

    }

    bool ParametersHaveValues()
    {
        if (om_tile.CentrePoint.HasValue && om_tile.Height.HasValue && om_tile.Width.HasValue
            && om_tile.Depth.HasValue && om_tile.Rotation.HasValue && om_tile.Level.HasValue)
        {
            return true;
        }
        return false;
    }
}
