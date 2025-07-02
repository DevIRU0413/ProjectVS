using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using ProjectVS;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D _rigid;
    private Animator _anim;
    private PlayerConfig _player;
    private PlayerAction _inputActions;

    public Vector2 MoveInput { get; private set; }
        
    public MapSwitcher MapSwitcher;
    public FadeManager Fade;
  
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _player = GetComponent<PlayerConfig>();

        _inputActions = new PlayerAction();
        _inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
    }

    void Start()
    {
        if (MapSwitcher == null)// 맵스위처 자동 할당
            MapSwitcher = FindObjectOfType<MapSwitcher>();
    }
    private void OnEnable()
    {
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    public void MovePlayer()
    {
        // rigidbody를 통해 이동
        _rigid.velocity = MoveInput.normalized * _player.Stats.CurrentSpd;

        // 이동 애니메이션 상태 설정
        bool isWalking = MoveInput != Vector2.zero;
        _anim.SetBool("IsWalking", isWalking);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door")) // 문과 충돌 했을시
        {
            Debug.Log("상점을 나감");
            StartCoroutine(HandleFadeTransition());
            // TODO : 씬 전환 필요 시 처리 추가
        }
    }
    private IEnumerator HandleFadeTransition()
    {
        yield return StartCoroutine(Fade.FadeOut()); // 페이드 아웃
        MapSwitcher.OnBattleField();  
        yield return StartCoroutine(Fade.FadeIn());
        
    }
   




}
