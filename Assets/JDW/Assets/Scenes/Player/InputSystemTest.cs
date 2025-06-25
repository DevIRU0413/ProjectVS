using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemTest : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("스페이스바 눌림");
        }

        if (Mouse.current != null)
        {
            Debug.Log("마우스 위치: " + Mouse.current.position.ReadValue());
        }
    }
}
