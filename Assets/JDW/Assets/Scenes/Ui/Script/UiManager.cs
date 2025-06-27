using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// 추후 Ui관련 팀원분과 조율해서 합칠예정
public class UiManager : MonoBehaviour
{
    public Image ExpBar;//경험치바를 채울 이미지
    public Image HpBar; //체력바를 채울 이미지
    public Player player;
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
            player = FindObjectOfType<Player>(); // 자동으로 플레이어 찾음       
        }

        // 플레이어나 스탯이 없으면 실행x
        if (player == null || player.stats == null) return;
        // 경험치 비율 
        float expRatio = player.stats.CurrentExp / player.stats.MaxExp;
        // 이미지 채움 정도 설정
        ExpBar.fillAmount = Mathf.Clamp01(expRatio);

        // 플레이어나 스탯이 없으면 실행x
        if (player == null || player.stats == null) return;
        // 체력 비율 
        float hpRatio = player.stats.Health / player.stats.MaxHealth;
        // 이미지 채움 정도 설정
        HpBar.fillAmount = Mathf.Clamp01(hpRatio);

        //플레이어 경험치 텍스트
        int currentExp = Mathf.FloorToInt(player.stats.CurrentExp);
        int maxExp = Mathf.FloorToInt(player.stats.MaxExp);
        expText.text = $"{currentExp}/{maxExp}";
        //플레이어 레벨 텍스트
        int currentLevel = Mathf.FloorToInt(player.stats.Level);
        levelText.text = $"LEVEL : {currentLevel}";
        //플레이어 골드 텍스트
        int currentGold = Mathf.FloorToInt(player.stats.Gold);
        goldText.text = $"{currentGold}";
        //플레이어 체력
        int currentHp = Mathf.FloorToInt(player.stats.Health);
        int maxHp = Mathf.FloorToInt(player.stats.MaxHealth);
        hpText.text = $"HP {currentHp}/{maxHp}";
    }
}
