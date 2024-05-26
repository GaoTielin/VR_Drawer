using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private int CheckPointSpace = 2;
    Color OriginColor;

    private void Start()
    {
        OriginColor = GetComponent<LineRenderer>().material.color;
    }

    public void SetCollider(BoxCollider colliderTemplate, float width)
    {
        lineRenderer = GetComponent<LineRenderer>();
        for(int i = 0; i < lineRenderer.positionCount - CheckPointSpace; i+= CheckPointSpace)
        {
            //TODO:�������м䴴��һ��collider��collider�ĳ����������ľ��룬�������������Է���
            // ��ȡ�������ڵ�
            Vector3 start = lineRenderer.GetPosition(i);
            Vector3 end = lineRenderer.GetPosition(i + CheckPointSpace);

            // ��������֮����е㡢����ͷ���
            Vector3 midPoint = (start + end) / 2;
            float distance = Vector3.Distance(start, end);
            Vector3 direction = (end - start).normalized;

            // �����µ�BoxCollider������������
            BoxCollider boxCollider = Instantiate(colliderTemplate, midPoint, Quaternion.identity, transform);
            boxCollider.size = new Vector3(width, width, distance);
            //boxCollider.transform.localScale = new Vector3(boxCollider.transform.localScale.x, boxCollider.transform.localScale.y, distance);

            // ��תBoxColliderʹ���׼�߶η���
            boxCollider.transform.rotation = Quaternion.LookRotation(direction);

            // ����λ��ʹ������λ���߶��м�
            boxCollider.transform.position = midPoint;
            boxCollider.gameObject.tag = "Line";
        }
    }

    public void Erased()
    {
        Destroy(gameObject);
    }

    public void ReadyToErase()
    {
        
        GetComponent<LineRenderer>().material.color = new Color(OriginColor.r / 2, OriginColor.g / 2, OriginColor.b / 2, OriginColor.a / 2);
    }

    public void ResetColor()
    {
        GetComponent<LineRenderer>().material.color = OriginColor;
    }
}
