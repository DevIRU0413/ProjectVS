using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigid;
    public Vector2 MoveInput;
    Animator anim;
    private Vector2? _blockedDirection = null;
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
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v).normalized;

        // 차단된 방향일 경우 입력 무시
        if (_blockedDirection.HasValue)
        {
            float dot = Vector2.Dot(input, _blockedDirection.Value);
            if (dot > 0.7f) // 같은 방향이면 막기
            {
                input = Vector2.zero;
            }
        }

        MoveInput = input;
        //   // W,A,S,D 상하좌우 이동
        //   MoveInput.x = Input.GetAxisRaw("Horizontal");
        //   MoveInput.y = Input.GetAxisRaw("Vertical");
        //대각선 이동 속도 유지
        MoveInput.Normalize();

        // 이동 애니메이션, 이동시 IsWalking을 ture로 바꿈
        bool IsWalking = MoveInput != Vector2.zero;
        anim.SetBool("IsWalking", IsWalking);
    }
    public void BlockDirection(Vector2 dir)
    {
        _blockedDirection = dir.normalized;
    }
    public void ClearBlockedDirection()
    {
        _blockedDirection = null;
    }
    private void OnTriggerEnter2D(Collider2D other) // 충돌 시작
    {
        if (other.CompareTag("Rocks")) // 바위와 충돌했을 때
        {
            Debug.Log("바위와 부딪힘");
            Vector2 hitDirection = other.transform.position - transform.position;
            BlockDirection(hitDirection); // 바위쪽 이동 방향 차단
        }
    }
    private void OnTriggerExit2D(Collider2D other) // 충돌 종료
    {
        if (other.CompareTag("Rocks")) 
            ClearBlockedDirection();
    }
}
