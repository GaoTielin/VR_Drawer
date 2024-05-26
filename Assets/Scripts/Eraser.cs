using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Eraser OnTriggerEnter --> " + other.tag);
        //��ײ��⣬ֻ��TagΪPen��������ܼ��
        if (other.tag == "DrawController")
        {
            other.GetComponent<DrawerController>().ChangeDraw();
        }
    }

}
