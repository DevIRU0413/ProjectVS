using UnityEngine;

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
