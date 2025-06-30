using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using ProjectVS;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigid;
    public Vector2 MoveInput;
    public Animator anim;
    public MapSwitcher mapSwitcher;

    private PlayerAction inputActions;

    public FadeManager fadeManager;
    public Timer timer;

    private PlayerConfig _player;
  
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _player = GetComponent<PlayerConfig>();

        inputActions = new PlayerAction();
        inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
    }
    void Start()
    {
        if (mapSwitcher == null)
            mapSwitcher = FindObjectOfType<MapSwitcher>();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void FixedUpdate()
    {
        playerMove();
    }
    public void playerMove()
    {
        // rigidbody를 통해 이동 , 게임 매니저를 통해 player의 move값을 가져옴
        rigid.velocity = MoveInput.normalized * _player.Stats.CurrentSpd;
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
            // TODO : 씬전환으로 진행할경우 코드추가
        }
    }
    private IEnumerator HandleFadeTransition()
    {
        yield return StartCoroutine(fadeManager.FadeOut()); // 페이드 아웃
        mapSwitcher.OnBattleField();  
        
        yield return StartCoroutine(fadeManager.FadeIn());
        
    }
   




}
