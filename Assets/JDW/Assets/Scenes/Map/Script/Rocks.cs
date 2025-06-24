using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{
    private PlayerMove _moveScript;

    private void Start()
    {
        _moveScript = GetComponent<PlayerMove>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rocks")) // 바위 오브젝트 태그 확인
        {
            Debug.Log("바위와 부딪힘");
            _moveScript.MoveInput = Vector2.zero; // 이동 정지
        }
    }
}
