using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardNav : MonoBehaviour
{
    #region Public Variables
    public float LerpTimeTranslation = 0.4f;
    public float LerpTimeRotation = 0.2f;
    public float UnitDistance = 1f;
    public float UnitRotationAngle = 7.5f;
    #endregion

    #region Private Variables
    private KeyCode[] movementKeys = { KeyCode.W, KeyCode.S, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    private KeyCode[] otherKeys = { KeyCode.RightShift, KeyCode.LeftShift };
    private bool movBlocked;
    private GameObject currentObject;
    #endregion
    /// </summary>

    private void Update()
    {
        // variables assigned only for poc
        movBlocked = gameObject.GetComponent<MovementController>().MovBlocked;
        currentObject = gameObject.GetComponent<MovementController>().CurrentObject;
        KeyboardNavigation();
    }

    private void KeyboardNavigation()
    {
        // Using keyboard for linear movement
        if (Input.anyKey && !movBlocked)
        {

            bool rot = true ? Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) : false;
            float lerpTime = rot ? LerpTimeRotation : LerpTimeTranslation;
            var unitP = new Vector3();
            var unitR = new Quaternion();
            var unitRInc = UnitRotationAngle;

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                unitP = UnitDistance * Vector3.back;
                unitR = Quaternion.Euler(-unitRInc, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                unitP = UnitDistance * Vector3.forward;
                unitR = Quaternion.Euler(unitRInc, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                unitP = UnitDistance * Vector3.left;
                unitR = Quaternion.Euler(0, unitRInc, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                unitP = UnitDistance * Vector3.right;
                unitR = Quaternion.Euler(0, -unitRInc, 0);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                unitP = UnitDistance * Vector3.up;
                unitR = Quaternion.Euler(0, 0, unitRInc);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                unitP = UnitDistance * Vector3.down;
                unitR = Quaternion.Euler(0, 0, -unitRInc);
            }

            foreach (var k in movementKeys)
            {
                if (Input.GetKeyDown(k))
                    StartCoroutine(moveOrRotateByUnit(currentObject.transform, unitP, unitR, lerpTime, rot));
            }

        }
    }

    private IEnumerator moveOrRotateByUnit(Transform objectToMove, Vector3 unitVector, Quaternion unitQuaternion, float lerpTime, bool rot)
    {
        movBlocked = true;
        var startPosition = objectToMove.localPosition;
        var startRotation = objectToMove.localRotation;
        var startTime = Time.time;

        while (Time.time - startTime < lerpTime)
        {
            if (rot)
            {
                var updatedRotation = Quaternion.Lerp(startRotation, startRotation * unitQuaternion, (Time.time - startTime) / lerpTime);
                objectToMove.localRotation = updatedRotation;
            }
            else
            {
                var updatedPosition = Vector3.Lerp(startPosition, unitVector + startPosition, (Time.time - startTime) / lerpTime);
                objectToMove.localPosition = updatedPosition;
            }
            yield return null;
        }

        //  option is to use just this instead too, if no sooth transition preferred
        //objectToMove.localPosition = unitVector + startPosition;
        movBlocked = false;
        yield return null;
    }
}
