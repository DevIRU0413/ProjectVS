using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigid;
    public Vector2 MoveInput;
    Animator anim;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        playerMove();
       
    }
    private void FixedUpdate()
    {
        // rigidbody를 통해 이동 , 게임 매니저를 통해 player의 move값을 가져옴
        rigid.velocity = MoveInput * GameManager.instance._player.MoveSpeed;
    }
    private void playerMove()
    {
        // W,A,S,D 상하좌우 이동
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");
        //대각선 이동 속도 유지
        MoveInput.Normalize();

        // 이동 애니메이션, 이동시 IsWalking을 ture로 바꿈
        bool IsWalking = MoveInput != Vector2.zero;
        anim.SetBool("IsWalking", IsWalking);
    }
}
