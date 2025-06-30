using ProjectVS;
using ProjectVS.Util;

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : SimpleSingleton<GameManager>
{
    public Timer timer;
    public MapSwitcer mapSwitcer;
    public PlayerStats playerStats;

    [HideInInspector] public Boss boss;
    [HideInInspector] public PlayerConfig Player;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public AttackPosition attackPosition;

    public GameObject[] playerPrefabs; // 0 = 검, 1 = 도끼, 2 = 마법
    public Transform playerSpawnPoint;
    public int CurrentClassIndex { get; private set; }

    private PlayerAction inputActions;
    private GameObject currentPlayerInstance;

    [SerializeField] GameObject Tilemap;

    protected override void Awake()
    {
        inputActions = new PlayerAction();
        inputActions.CharacterSelect.Enable();
        inputActions.CharacterSelect.SelectClass.performed += OnClassSelect;
    }
    protected override void OnDestroy()
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
        Player = currentPlayerInstance.GetComponent<PlayerConfig>();
        playerMove = currentPlayerInstance.GetComponent<PlayerMove>();

        UiManager ui = FindObjectOfType<UiManager>();
        if (ui != null)
        {
            ui.player = Player; // 생성된 플레이어를 ui매니저한테 줌
            ui.boss = boss; // 생성된 보스를 ui매니저한테 줌
        }



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
   
    private void StopTile() // 씬 전환으로 타일이 제대로 깔리면 필요없음
    {

        if (timer != null && timer.currentTime <= 1f) // 타이머가 널이 아니고 현재 시간이 1초가 되었을 때
            foreach (var r in Tilemap.GetComponentsInChildren<Reposition>()) // 리포지션 스크립트를 비활성화
                r.isActive = false;
    }
    private void StratrTile()
    {
        if (mapSwitcer.IsStoreActive())
        {
            foreach (var r in Tilemap.GetComponentsInChildren<Reposition>()) // 스토어가 활성화 되었을 때 다시 스크립트 활성화
                r.isActive = true;
        }
    }

}

