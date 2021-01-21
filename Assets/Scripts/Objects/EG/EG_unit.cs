using Microsoft.CodeAnalysis;
using OM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EG_unit : MonoBehaviour
{
    public OM_unit om_unit = new OM_unit();


    Vector3? centrePoint;
    float? width;
    float? height;
    float? rotation;


    void Start()
    {
        StartCoroutine(WaitAndExectute());

        this.name = "Tile Unit";
        this.tag = "tile";
        this.transform.position = om_unit.CentrePoint;
        this.transform.localScale = new Vector3(om_unit.Width, om_unit.Height, om_unit.Width);
        this.transform.Rotate(0, om_unit.Rotation, 0);
    }
    void addCubeGameObject()
    {
        this.name = "Tile Unit";
        this.tag = "tile";
        this.transform.position = om_unit.CentrePoint;
        this.transform.localScale = new Vector3(om_unit.Width, om_unit.Height, om_unit.Width);
        this.transform.Rotate(0, om_unit.Rotation, 0);
    }

    IEnumerator WaitAndExectute()
    {
        Debug.Log("Courutine entered");
        yield return new WaitWhile(() => ParametersHaveValues());
    }

    bool ParametersHaveValues()
    {
        if (this.centrePoint.HasValue && this.height.HasValue && this.width.HasValue
            && this.rotation.HasValue)
        {
            return true;
        }

        return false;
    }
}
