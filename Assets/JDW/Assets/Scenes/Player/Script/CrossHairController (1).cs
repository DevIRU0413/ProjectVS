using System.Collections;
using System.Collections.Generic;

using ProjectVS.JDW;

using UnityEngine;

using static UnityEditor.Experimental.GraphView.GraphView;

public class CrossHairController : MonoBehaviour
{
    private void Update()
    {
        MoveToMousePos();
    }

    private void MoveToMousePos()
    {
        if (Camera.main == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        transform.position = mousePos;
    }
}
