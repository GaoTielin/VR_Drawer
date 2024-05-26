using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEraser : MonoBehaviour
{
    Line TouchLine = null;

    UnityEngine.XR.InputDevice RightHandDevice;

    private void Start()
    {
        //获取右手输入设备
        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);

        if (rightHandDevices.Count == 1)
        {
            RightHandDevice = rightHandDevices[0];
            Debug.LogError(string.Format("Device name '{0}' with role '{1}'", RightHandDevice.name, RightHandDevice.role.ToString()));
        }
        else if (rightHandDevices.Count > 1)
        {
            Debug.LogError("Found more than one left hand!");
        }
        if (RightHandDevice == null)
        {
            Debug.LogError("Cant Find RightHandDevice");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Line")
        {
            /*TouchLine = other.GetComponent<Line>();
            if(TouchLine == null)
            {
                TouchLine = other.GetComponentInParent<Line>();
                TouchLine.ReadyToErase();
            }*/
            TouchLine = GetLineByCollider(other);

            if (TouchLine == null)
            {
                return;
            }

            TouchLine.ReadyToErase();
        }
    }

    void Update()
    {
        if (CheckRightInputTrigger() || Input.GetKeyDown(KeyCode.A))
        {
            EraseLine();
        }
    }

    private bool CheckRightInputTrigger()
    {
        bool triggerValue;
        if (RightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {
            return true;
        }
        return false;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Line")
        {
            Line line = GetLineByCollider(other);
            if (line != null && line.Equals(TouchLine) )
            {
                TouchLine.ResetColor();
                TouchLine = null;
            }
        }
    }

    private Line GetLineByCollider(Collider other)
    {
        TouchLine = other.GetComponent<Line>();
        if (TouchLine == null)
        {
            TouchLine = other.GetComponentInParent<Line>();
            
        }
        return TouchLine;
    }

    private void EraseLine()
    {
        if(TouchLine == null)
        {
            return;
        }

        TouchLine.Erased();

        TouchLine = null;
    }
}
