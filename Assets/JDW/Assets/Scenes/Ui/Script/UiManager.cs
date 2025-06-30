using ProjectVS;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
// 추후 Ui관련 팀원분과 조율해서 합칠예정
public class UiManager : MonoBehaviour
{
    public PixelUI.ValueBar ExpBar;//경험치바를 채울 이미지
    public PixelUI.ValueBar BossHpBar;//보스 체력바를 채울 이미지
    public PlayerConfig player;
    public Boss boss;

    public TextMeshProUGUI levelText; // 레벨 
    public TextMeshProUGUI goldText;//골드
    private void Awake()
    {
        player = FindObjectOfType<PlayerConfig>();
        boss = FindObjectOfType<Boss>();
    }
    private void Start()
    {
        //초기에 플레이어가 없을 경우 대체값
        if (ExpBar != null)
        {
            ExpBar.CurrentValue = 0f;
            ExpBar.MaxValue = 1f; // 비율 기반이라서 0~1
        }
    }

    private void Update()
    {
        if (player == null)
            player = FindObjectOfType<PlayerConfig>(); // 자동으로 플레이어 찾음       
        if (boss == null)
            boss = FindObjectOfType<Boss>(); // 자동으로 보스를 찾음       

        // 플레이어나 스탯이 없으면 실행x
        if (player == null || player.Stats == null) return;
        // 경험치 비율 
        float expRatio = player.Stats.CurrentExp / player.Stats.MaxExp;
        if (ExpBar != null)
        {
            ExpBar.MaxValue = player.Stats.MaxExp;
            ExpBar.CurrentValue = player.Stats.CurrentExp;
        }
        // 보스 체력 비율
        if (boss != null && BossHpBar != null)
        {
            float bossHpRatio = boss.currentHp / boss.maxHp;
            BossHpBar.MaxValue = boss.maxHp;
            BossHpBar.CurrentValue = boss.currentHp;
        }
        // 플레이어 레벨, 골드 텍스트
        levelText.text = $"{Mathf.FloorToInt(player.Stats.Level)}";
        goldText.text = $"{Mathf.FloorToInt(player.Stats.Gold)}";
    }
}
