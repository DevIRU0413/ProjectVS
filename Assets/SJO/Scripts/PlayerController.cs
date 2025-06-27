using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 움직이는 속도
    public float _moveSpeed;
    private Rigidbody2D _rigid;

    public Scanner scanner;

    private Vector2 InputVec;
    private float _inputX;

    // <예외 처리>
    // 땅에 닿았는지를 판단하기 위한 bool 타입 변수
    private bool _isGrounded;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerInput()
    {
        _inputX = Input.GetAxis("Horizontal");
    }

    private void PlayerMove()
    {
        _rigid.velocity = new Vector2(_inputX * _moveSpeed, _rigid.velocity.y);
    }
}
