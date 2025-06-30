using ProjectVS;
using ProjectVS.Data;
using ProjectVS.Manager;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
// 추후 Ui관련 팀원분과 조율해서 합칠예정
public class UiManager : MonoBehaviour
{
    public Image ExpBar;//경험치바를 채울 이미지
    public Image HpBar; //체력바를 채울 이미지
    public PlayerConfig player;
    public TextMeshProUGUI expText; // 경험치 
    public TextMeshProUGUI levelText; // 레벨 
    public TextMeshProUGUI hpText;// 체력
    public TextMeshProUGUI goldText;//골드
    private void Start()
    {
        //초기에 플레이어가 없을 경우 대체값
        ExpBar.fillAmount = 0f;
        expText.text = "0 / ?";
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerConfig>(); // 자동으로 플레이어 찾음       
        }

        // 플레이어나 스탯이 없으면 실행x
        if (player == null || player.Stats == null) return;
        // 경험치 비율 
        float expRatio = player.Stats.CurrentExp / player.Stats.MaxExp;
        // 이미지 채움 정도 설정
        ExpBar.fillAmount = Mathf.Clamp01(expRatio);

        // 플레이어나 스탯이 없으면 실행x
        if (player == null || player.Stats == null) return;
        // 체력 비율 
        float hpRatio = player.Stats.CurrentHp / player.Stats.CurrentMaxHp;
        // 이미지 채움 정도 설정
        HpBar.fillAmount = Mathf.Clamp01(hpRatio);

        //플레이어 경험치 텍스트
        int currentExp = Mathf.FloorToInt(player.Stats.CurrentExp);
        int maxExp = Mathf.FloorToInt(player.Stats.MaxExp);
        expText.text = $"{currentExp}/{maxExp}";
        //플레이어 레벨 텍스트
        int currentLevel = Mathf.FloorToInt(player.Stats.Level);
        levelText.text = $"LEVEL : {currentLevel}";
        //플레이어 골드 텍스트
        int currentGold = Mathf.FloorToInt(PlayerDataManager.Instance.gold);
        goldText.text = $"{currentGold}";
        //플레이어 체력
        int currentHp = Mathf.FloorToInt(player.Stats.CurrentHp);
        int maxHp = Mathf.FloorToInt(player.Stats.CurrentMaxHp);
        hpText.text = $"HP {currentHp}/{maxHp}";
    }
}
