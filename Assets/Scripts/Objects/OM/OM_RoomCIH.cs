using System;
using System.Collections.Generic;
using UnityEngine;
using BH.oM.Base;
//using BH.Engine.CIH;
using BH.oM.CIH.Conditions;
using BH.oM.Data.Specifications;


public class RoomObj:IObject
{
    public float Area = 51f;
    public string TypologyName = "Office";
}
public class RoomCIH : MonoBehaviour, IObject
{
    public string TypologyName { get; set; }
    /*
    public float MinArea { get; set; }
    public bool IsCirculation { get; set; }
    public float DaylightFactor { get; set; }
    public int AcousticSeparation { get; set; }
    */
    
    /// <remarks>
    /// The following properties are associated with the 
    /// individual rooms generated in Module 2
    /// Polyline can be replaced with list of tuples
    /// </remarks>
    public string RoomName { get; set; }
    public List<(float, float)> Outline { get; set; }
    public List<bool> EdgesExternal { get; set; }
    public float Height { get; set; }
    public int Level { get; set; }
    public float Area { get; set; }
    public BH.oM.Data.Specifications.Specification spec = new BH.oM.Data.Specifications.Specification();
    /// <remarks>
    ///  Testing integration with CIH Engine and BHoM references for specification
    /// </remarks>
    public void VerifySpecTest()
    {
        IsGreaterThan AreaCondition = new IsGreaterThan();
        AreaCondition.PropertyName = "Area";
        AreaCondition.ReferenceValue = 50f;

        IsEqualTo NameCondition = new IsEqualTo();
        NameCondition.PropertyName = "TypologyName";
        NameCondition.ReferenceValue = "Office";

        RoomObj roomObj = new RoomObj();
        spec.CheckConditions = new List<ICondition> { AreaCondition};
        spec.FilterConditions = new List<ICondition> { NameCondition };
        try
        {
            SpecificationResult specificationResult = BH.Engine.CIH.Compute.VerifySpecifications(new List<object> { roomObj }, new List<ISpecification> { spec });
            Debug.Log(specificationResult.PassedObjects.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Exception caught: " + e.ToString());
        }

    }

    
    
}
