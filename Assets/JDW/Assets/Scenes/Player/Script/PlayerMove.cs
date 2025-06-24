using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigid;
    public Vector2 MoveInput;
    Animator anim;
    private Vector2? _blockedDirection = null;
    public FadeManager fadeManager;
    public Timer timer;
    private bool _isFading = false;

    public GameObject BattleField;
    public GameObject StoreField;
    public GameObject player; // 이동시킬 플레이어
    
    public GameObject Tilemap1;
    public GameObject Tilemap2;
    public GameObject Tilemap3;
    public GameObject Tilemap4;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Tilemap tilemap = timer.BattleField.GetComponentInChildren<Tilemap>();
        
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
        if (other.CompareTag("Obstacle")) // 장애물 오브젝트와 충돌했을 때
        {
            Debug.Log("바위와 부딪힘");
            Vector2 hitDirection = other.transform.position - transform.position;
            BlockDirection(hitDirection); // 장애물 오브젝트쪽 이동 방향 차단
        }
        else if (other.CompareTag("Door"))
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
        yield return StartCoroutine(fadeManager.FadeOut());
        timer.StoreField.SetActive(false); // 페이드인 이후 스토어필드 오프
        timer.BattleField.SetActive(true); // 페이드아웃 이후 배틀필드 온
        player.transform.position = new Vector3(0f, 0f, -1f); //플레이어의 위치 초기화
        DisableTilemapTriggers();
        
        
        yield return StartCoroutine(fadeManager.FadeIn());
        

        _isFading = false;
    }
    private void DisableTilemapTriggers()
    {
        Tilemap1.GetComponent<Reposition>().isActive = true; // 무한맵 스크립트 재가동
        Tilemap2.GetComponent<Reposition>().isActive = true;
        Tilemap3.GetComponent<Reposition>().isActive = true;
        Tilemap4.GetComponent<Reposition>().isActive = true;
        Tilemap1.transform.position = new Vector3(24f, 11f, 0f);
        Tilemap2.transform.position = new Vector3(-14f, 11f, 0f);
        Tilemap3.transform.position = new Vector3(-16f,-29f, 0f);
        Tilemap4.transform.position = new Vector3(26f, -29f, 0f);
    }


}
