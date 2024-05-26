using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [SerializeField]
    public Color TargetColor;

    // Start is called before the first frame update
    void Start()
    {
        //将自身的颜色设置为目标颜色
        transform.GetComponent<Renderer>().material.color = TargetColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        //碰撞检测，只有Tag为Pen的物体接受检测
        if(other.tag == "Pen")
        {
            other.GetComponent<LineDrawer>().SetDrawColor(TargetColor);
        }
    }
}
