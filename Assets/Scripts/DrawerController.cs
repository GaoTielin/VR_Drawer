using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [SerializeField]
    LineDrawer drawer;
    [SerializeField]
    LineEraser eraser;

    bool IsDraw = true;

    private void Start()
    {
        SetDrawer();
    }

    public void SetDrawer()
    {
        eraser.gameObject.SetActive(false);
        drawer.gameObject.SetActive(true);
        IsDraw = true;
    }

    public void SetEraser()
    {
        eraser.gameObject.SetActive(true);
        drawer.gameObject.SetActive(false);
        IsDraw = false;
    }

    public void ChangeDraw()
    {
        if(IsDraw)
        {
            SetEraser();
        }
        else
        {
            SetDrawer();
        }
        Debug.LogError("ChangeDraw ---> " + IsDraw);
    }
}
