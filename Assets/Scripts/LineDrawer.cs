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
        //��ȡ���������豸
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
    void Update() //ÿ֡����
    {

        //���°���ʱ����ͼ�β�����滭״̬
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

        //����һ�����壬���Ҹ��丽��lineRednder���
        line = new GameObject().AddComponent<LineRenderer>();
        lineComp = line.gameObject.AddComponent<Line>();
        line.positionCount = 0;

        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        //���ò���ʱ��������ɫ����Ϊ��ǰ��ɫ
        line.material = mat;
        line.GetComponent<LineRenderer>().material.SetColor("_BaseColor", NowDrawColor);

        lastPos = HandTrans.position; //��¼��һ֡��λ��

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
        //�ж���֮֡���ֱ�֮��ľ��룬����3���ײŻ��ƣ���Ȼ�ᵼ�µ����������
        if(Vector3.Distance(lastPos, HandTrans.position) > 0.01f)
        {
            RealTimeDrwaLine(line, HandTrans.position);
            lastPos = HandTrans.position; //��¼��ǰ֡��λ�ã�������һ֡�ľ����ж�
        }
    }

    //ʵʱ��������λ��
    private void RealTimeDrwaLine(LineRenderer lineRenderer, Vector3 newPoint)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);
    }

    //�޸Ļ��Ƶ���ɫ
    public void SetDrawColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", color);
        NowDrawColor = color; //mat.SetColor("_BaseColor", color);
    }

    //�Զ���һ����
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
