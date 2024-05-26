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
            //TODO:在两点中间创建一个collider，collider的长度是两点间的距离，方向是两点的相对方向
            // 获取两个相邻点
            Vector3 start = lineRenderer.GetPosition(i);
            Vector3 end = lineRenderer.GetPosition(i + CheckPointSpace);

            // 计算两点之间的中点、距离和方向
            Vector3 midPoint = (start + end) / 2;
            float distance = Vector3.Distance(start, end);
            Vector3 direction = (end - start).normalized;

            // 创建新的BoxCollider并设置其属性
            BoxCollider boxCollider = Instantiate(colliderTemplate, midPoint, Quaternion.identity, transform);
            boxCollider.size = new Vector3(width, width, distance);
            //boxCollider.transform.localScale = new Vector3(boxCollider.transform.localScale.x, boxCollider.transform.localScale.y, distance);

            // 旋转BoxCollider使其对准线段方向
            boxCollider.transform.rotation = Quaternion.LookRotation(direction);

            // 调整位置使其中心位于线段中间
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
