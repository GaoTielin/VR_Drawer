using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField]
    public Transform HandTrans;
    [SerializeField]
    public Material mat;
    [SerializeField]
    public BoxCollider LineCollider;

    private bool isDraw;
    private LineRenderer line;
    private Line lineComp;

    private Vector3 lastPos;
    UnityEngine.XR.InputDevice RightHandDevice;

    private Color NowDrawColor;

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
        if(RightHandDevice == null)
        {
            Debug.LogError("Cant Find RightHandDevice");
        }

        SetDrawColor(Color.blue);
    }


    // Update is called once per frame
    void Update() //每帧更新
    {

        //按下按键时创建图形并进入绘画状态
        if (CheckRightInputTrigger() || Input.GetKey(KeyCode.A))
        {
            if(isDraw)
            {
                DrawUpdate();
            }
            else
            {
                StartDraw();
            }

        }
        if((!CheckRightInputTrigger() && !Input.GetKey(KeyCode.A)) && isDraw)
        {
            StopDraw();
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            AutoDrawOneLine();
        }
    }

    private void ChangeDrawState()
    {
        if(!isDraw)
        {
            StartDraw();
        }
        else
        {
            StopDraw();
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

    private void StartDraw()
    {
        isDraw = true;

        //创建一个物体，并且给其附加lineRednder组件
        line = new GameObject().AddComponent<LineRenderer>();
        lineComp = line.gameObject.AddComponent<Line>();
        line.positionCount = 0;

        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        //设置材质时将材质颜色设置为当前颜色
        line.material = mat;
        line.GetComponent<LineRenderer>().material.SetColor("_BaseColor", NowDrawColor);

        lastPos = HandTrans.position; //记录上一帧的位置

        Debug.LogError("StartDraw-----");
    }

    private void StopDraw()
    {
        isDraw = false;
        if(lineComp != null)
        {
            lineComp.SetCollider(LineCollider, line.startWidth);
            lineComp.gameObject.tag = "Line";
        }
        Debug.LogError("StopDraw-----");
    }

    private void DrawUpdate()
    {
        //判断两帧之间手柄之间的距离，超过3厘米才绘制，不然会导致点的数量激增
        if(Vector3.Distance(lastPos, HandTrans.position) > 0.01f)
        {
            RealTimeDrwaLine(line, HandTrans.position);
            lastPos = HandTrans.position; //记录当前帧的位置，用作下一帧的距离判断
        }
    }

    //实时绘制线条位置
    private void RealTimeDrwaLine(LineRenderer lineRenderer, Vector3 newPoint)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);
    }

    //修改绘制的颜色
    public void SetDrawColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", color);
        NowDrawColor = color; //mat.SetColor("_BaseColor", color);
    }

    //自动画一条线
    private void AutoDrawOneLine()
    {
        StartDraw();
        StartCoroutine(AutoDraw());
    }

    IEnumerator AutoDraw()
    {
        int count = 0;
        while (count < 60)
        {
            yield return new WaitForEndOfFrame();
            count++;
            RealTimeDrwaLine(line, HandTrans.position + new Vector3(0.01f*count, 0, 0));
            lastPos = HandTrans.position; 

        }
    }
}
