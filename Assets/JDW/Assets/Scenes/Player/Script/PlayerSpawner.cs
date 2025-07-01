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

    public GameObject[] playerPrefabs; // 0 = 검, 1 = 도끼, 2 = 마법
    public Transform playerSpawnPoint;
    public int CurrentClassIndex { get; private set; }

    private PlayerAction inputActions;
    private GameObject currentPlayerInstance;
    private List<CharacterSelectionDataClass> characterDataList;// TSV에서 파싱된 캐릭터 정보

    private void Awake()
    {
        //TSV 데이터 불러옴
        CsvTable table = new CsvTable("Min/Resources/CharacterSelectionData.tsv", '\t');
        characterDataList = CharacterSelectionDataParser.Parse(table);

        inputActions = new PlayerAction(); // 인풋액션 등록
        inputActions.CharacterSelect.Enable();
        inputActions.CharacterSelect.SelectClass.performed += OnClassSelect;
     
    }
    private void OnDestroy()
    {
        inputActions.CharacterSelect.SelectClass.performed -= OnClassSelect; // 한번 입력후 이벤트 해제
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
        // 프리팹 생성
        GameObject newPlayer = Instantiate(playerPrefabs[index], playerSpawnPoint.position, Quaternion.identity);
        // playerConfig 컴포넌트 가져옴
        PlayerConfig config = newPlayer.GetComponent<PlayerConfig>();
        if (config != null && index < characterDataList.Count)
        {
            config.ApplyStatsFromData(characterDataList[index], index);// TSV와 클래스 정보 공격방식을 적용
        }
        // 현재 클래스 인덱스 저장
        CurrentClassIndex = index;
    }
}
