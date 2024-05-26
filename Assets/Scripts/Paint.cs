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
        //���������ɫ����ΪĿ����ɫ
        transform.GetComponent<Renderer>().material.color = TargetColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        //��ײ��⣬ֻ��TagΪPen��������ܼ��
        if(other.tag == "Pen")
        {
            other.GetComponent<LineDrawer>().SetDrawColor(TargetColor);
        }
    }
}
