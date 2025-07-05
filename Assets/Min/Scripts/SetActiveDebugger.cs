using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveDebugger : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"[SetActive 추적] {gameObject.name} 활성화됨\n{Environment.StackTrace}");
    }

    private void OnDisable()
    {
        Debug.Log($"[SetActive 추적] {gameObject.name} 이(가) 비활성화됨 (SetActive false)");
    }
}
