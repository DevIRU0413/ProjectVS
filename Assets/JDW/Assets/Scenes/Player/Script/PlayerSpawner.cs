using System.Collections;
using System.Collections.Generic;
using ProjectVS;

using UnityEngine;
using UnityEngine.InputSystem;
using ProjectVS.CharacterSelectionData.CharacterSelectionDataParser;
using CharacterSelectionDataClass = ProjectVS.CharacterSelectionData.CharacterSelectionData.CharacterSelectionData;
using ProjectVS.Utils.CsvTable;
public class PlayerSpawner : MonoBehaviour 
{
    [HideInInspector] public PlayerConfig Player;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public AttackPosition attackPosition;

    public GameObject[] playerPrefabs; // 0 = 검, 1 = 도끼, 2 = 마법
    public Transform playerSpawnPoint;
    public int CurrentClassIndex { get; private set; }

    private PlayerAction inputActions;
    private GameObject currentPlayerInstance;
    private List<CharacterSelectionDataClass> characterDataList;

    private void Awake()
    {
        //TSV 데이터 불러옴
        CsvTable table = new CsvTable("Min/Resources/CharacterSelectionData.tsv", '\t');
        characterDataList = CharacterSelectionDataParser.Parse(table);

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
        Player = currentPlayerInstance.GetComponent<PlayerConfig>();
        playerMove = currentPlayerInstance.GetComponent<PlayerMove>();

        attackPosition = currentPlayerInstance.GetComponentInChildren<AttackPosition>();
        if (attackPosition == null)
        {
            Debug.LogError("AttackPosition 컴포넌트가 프리팹에 없음!");
            return;
        }
        if (index < characterDataList.Count)
        {
            Player.ApplyStatsFromData(characterDataList[index]);
        }
        else
        {
            Debug.LogWarning("TSV 데이터에 해당 인덱스 정보가 없습니다.");
        }

        // 번호에 따라 공격 코루틴 자동 실행
        switch (index)
        {
            case 0: attackPosition.SwitchCoroutine(attackPosition.Axe()); break;
            case 1: attackPosition.SwitchCoroutine(attackPosition.Sword()); break;
            case 2: attackPosition.SwitchCoroutine(attackPosition.Fire()); break;
        }
    
        CurrentClassIndex = index;
    }
}
