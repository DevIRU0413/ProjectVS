using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public PlayerMove playerMove;
    public Bullet bullet;
    public Timer timer;
    public MapSwitcer mapSwitcer;
    public AttackPosition attackPosition;

    private string _bossTag = "Boss";
    private string _storeTag = "Store";

    public GameObject[] playerPrefabs; // 0 = 검, 1 = 도끼, 2 = 마법
    public Transform playerSpawnPoint;
    public int CurrentClassIndex { get; private set; }

    private PlayerAction inputActions;
    private GameObject currentPlayerInstance;

    [SerializeField] GameObject Boss;
    [SerializeField] GameObject Store;
    [SerializeField] GameObject Tilemap;
    private void Awake()
    {
        instance = this;
        inputActions = new PlayerAction();
        inputActions.CharacterSelect.Enable();
        inputActions.CharacterSelect.SelectClass.performed += OnClassSelect;
    }
    private void OnDestroy()
    {
        
        inputActions.CharacterSelect.SelectClass.performed -= OnClassSelect;
    }
    private void OnClassSelect(InputAction.CallbackContext ctx)
    {
        if (currentPlayerInstance != null) return; // 이미 플레이어가 생성되었으면 무시

        string key = ctx.control.displayName; // 입력된 키 문자열 ("1", "2", "3")
        int index = -1;

        // 키에 따라 프리팹 인덱스 결정
        switch (key)
        {
            case "1": index = 0; break;
            case "2": index = 1; break;
            case "3": index = 2; break;
        }

        if (index >= 0 && index < playerPrefabs.Length)
            SpawnPlayer(index); // 해당 인덱스의 클래스 생성
    }
    private void SpawnPlayer(int index)
    {
        currentPlayerInstance = Instantiate(playerPrefabs[index], playerSpawnPoint.position, Quaternion.identity);
        player = currentPlayerInstance.GetComponent<Player>();
        playerMove = currentPlayerInstance.GetComponent<PlayerMove>();

        attackPosition = currentPlayerInstance.GetComponentInChildren<AttackPosition>();
        if (attackPosition == null)
        {
            Debug.LogError("AttackPosition 컴포넌트가 프리팹에 없음!");
            return;
        }

        // 번호에 따라 공격 코루틴 자동 실행
        switch (index)
        {
            case 0:
                attackPosition.SwitchCoroutine(attackPosition.Axe());
                break;
            case 1:
                attackPosition.SwitchCoroutine(attackPosition.Sword());
                break;
            case 2:
                attackPosition.SwitchCoroutine(attackPosition.Fire());
                break;
        }
        CurrentClassIndex = index;
    }

    private void Update()
    {
        TimeTxet();
        StopTile();
        StratrTile();
    }
    private void TimeTxet()
    {
        GameObject boss = GameObject.FindWithTag(_bossTag);// 보스 태그를 확인
        if (boss != null) { timer.timerText.text = "Boss!"; timer.PauseTimer(); return; }//시간을 멈추고 그 태그로 표기
        GameObject store = GameObject.FindWithTag(_storeTag);// 상점 태그를 확인
        if (store != null) { timer.timerText.text = "$Store$"; timer.PauseTimer(); return; }//시간을 멈추고 그 태그로 표기
        timer.ResumeTimer();
    }
    private void StopTile()
    {

        if (timer != null && timer.currentTime <= 1f) // 타이머가 널이 아니고 현재 시간이 1초가 되었을 때
            foreach (var r in Tilemap.GetComponentsInChildren<Reposition>()) // 리포지션 스크립트를 비활성화
                r.isActive = false;
    }
    private void StratrTile()
    {
        if (mapSwitcer.IsStoreActive())
        {
            foreach (var r in Tilemap.GetComponentsInChildren<Reposition>())
                r.isActive = true;
        }
    }

}

