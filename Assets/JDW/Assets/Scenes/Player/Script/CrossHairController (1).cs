using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairController : MonoBehaviour
{
    private void Update()
    {
        MoveToMousePos();
    }

    private void MoveToMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        transform.position = mousePos;
    }
}
