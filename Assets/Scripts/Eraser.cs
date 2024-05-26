using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Eraser OnTriggerEnter --> " + other.tag);
        //碰撞检测，只有Tag为Pen的物体接受检测
        if (other.tag == "DrawController")
        {
            other.GetComponent<DrawerController>().ChangeDraw();
        }
    }

}
