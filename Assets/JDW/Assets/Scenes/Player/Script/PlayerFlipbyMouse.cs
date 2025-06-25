using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlipbyMouse : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer; // 플레이어의 이미지를 제어할 컴포넌트
    private Camera _mainCamera;  // 마우스 좌표를 계산할 카메라

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // 메인 카메라 가져오기
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        /*
        
        // 마우스의 화면 좌표를 원드 좌표로 변환
        Vector3 _mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // 마우스가 플레이어 왼쪽에 있을때 flipx을 true로 변경 => 이미지가 반전됨
        // 오른쪽에 있을경우 false로 변경 => 이미지 반전이 풀림
        _spriteRenderer.flipX = _mouseWorldPos.x > transform.position.x;

        */
    }

    public void OnMouse(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        // 마우스의 화면 좌표를 원드 좌표로 변환
        Vector3 _mouseWorldPos = _mainCamera.ScreenToWorldPoint(input);

        // 마우스가 플레이어 왼쪽에 있을때 flipx을 true로 변경 => 이미지가 반전됨
        // 오른쪽에 있을경우 false로 변경 => 이미지 반전이 풀림
        _spriteRenderer.flipX = _mouseWorldPos.x > transform.position.x;
    }
}
