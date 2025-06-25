using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigid;
    public MapSwitcer mapSwitcer;
    public Vector2 MoveInput;
    public Animator anim;
    private Vector2? _blockedDirection = null;

    public FadeManager fadeManager;
    public Timer timer;

    private bool _isFading = false;
  
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
        rigid.velocity = MoveInput * GameManager.instance.player.MoveSpeed;
    }
    private void playerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v).normalized;

        // 차단된 방향일 경우 입력 무시
      // if (_blockedDirection.HasValue)
      // {
      //     float dot = Vector2.Dot(input, _blockedDirection.Value);
      //     if (dot > 0.7f) // 같은 방향이면 막기
      //     {
      //         input = Vector2.zero;
      //     }
      // }

        MoveInput = input;

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
        if (other.CompareTag("Door")) // 문과 충돌 했을시
        {
            Debug.Log("상점을 나감");
            StartCoroutine(HandleFadeTransition());
        }
    }
    private void OnTriggerExit2D(Collider2D other) // 충돌 종료
    {
        if (other.CompareTag("Obstacle")) 
            ClearBlockedDirection();
    }
    private IEnumerator HandleFadeTransition()
    {
        _isFading = true;
        yield return StartCoroutine(fadeManager.FadeOut()); // 페이드 아웃
        mapSwitcer.OnBattleField(); // 배틀 온/ 상점 오프
        PlayerPositionReset();
        mapSwitcer.ResetTileMap(); // 상점에서 나왔을 때 타일들의 초기 위치
        
        
        yield return StartCoroutine(fadeManager.FadeIn());
        

        _isFading = false;
    }
    public void PlayerPositionReset()
    {
        transform.position = new Vector3(0f, 0f, -1f); // 플레이어의 위치 초기화
    }




}
