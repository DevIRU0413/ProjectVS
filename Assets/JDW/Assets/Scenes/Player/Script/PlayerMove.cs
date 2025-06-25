using System.Collections;

using ProjectVS.Manager;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigid;
    public MapSwitcer mapSwitcer;
    public Vector2 MoveInput;
    public Animator anim;

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
        // playerMove();
    }
    private void FixedUpdate()
    {
        // rigidbody를 통해 이동 , 게임 매니저를 통해 player의 move값을 가져옴
        rigid.velocity = MoveInput * GameManager.Instance.player.MoveSpeed;
    }
    private void playerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v).normalized;



        MoveInput = input;

        //대각선 이동 속도 유지
        MoveInput.Normalize();

        // 이동 애니메이션, 이동시 IsWalking을 ture로 바꿈
        bool IsWalking = MoveInput != Vector2.zero;
        anim.SetBool("IsWalking", IsWalking);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door")) // 문과 충돌 했을시
        {
            Debug.Log("상점을 나감");
            StartCoroutine(HandleFadeTransition());
        }
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

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        input.Normalize();
        MoveInput = input;

        bool IsWalking = MoveInput != Vector2.zero;
        anim.SetBool("IsWalking", IsWalking);
    }
}
