using ProjectVS;
using ProjectVS.Data;
using ProjectVS.Manager;

using TMPro;

using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;
using UnityEngine.UI;
namespace ProjectVS.Utils.UIManager
{
    // 추후 Ui관련 팀원분과 조율 예정
    public class UiManager : MonoBehaviour
    {
        public PixelUI.ValueBar ExpBar;//경험치바를 채울 이미지
        public PixelUI.ValueBar BossHpBar;//보스 체력바를 채울 이미지
        public PlayerConfig Player;
        public Boss Boss;
        public PlayerDataManager PlayerDataManager;

        public TextMeshProUGUI LevelText; // 레벨 
        public TextMeshProUGUI GoldText;//골드

        [SerializeField] private Sprite[] _hpPortraits;
        [SerializeField] private Image _portraitImage;
        private void Awake()
        {
            Player = FindObjectOfType<PlayerConfig>();
            Boss = FindObjectOfType<Boss>();
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
            if (Player == null)
                Player = FindObjectOfType<PlayerConfig>(); // 자동으로 플레이어 찾음       
            if (Boss == null)
                Boss = FindObjectOfType<Boss>(); // 자동으로 보스를 찾음


            UpdateExpBar();
            UpdateBossHpBar();
            UpdateTexts();
        }
        public void UpdatePortrait(float hpRatio)
        {

            hpRatio = Mathf.Clamp01(hpRatio);

            if (hpRatio > 0.8f)
                _portraitImage.sprite = _hpPortraits[4];
            else if (hpRatio > 0.6f)
                _portraitImage.sprite = _hpPortraits[3];
            else if (hpRatio > 0.4f)
                _portraitImage.sprite = _hpPortraits[2];
            else if (hpRatio > 0.2f)
                _portraitImage.sprite = _hpPortraits[1];
            else
                _portraitImage.sprite = _hpPortraits[0];
        }

        private void UpdateExpBar()
        {
            if (ExpBar == null || Player == null || Player.Stats == null)
                return;

            ExpBar.MaxValue = Player.Stats.MaxExp;
            ExpBar.CurrentValue = Player.Stats.CurrentExp;
        }

        private void UpdateBossHpBar()
        {
            if (Boss == null || BossHpBar == null)
                return;

            BossHpBar.MaxValue = Boss.maxHp;
            BossHpBar.CurrentValue = Boss.currentHp;
        }

        private void UpdateTexts()
        {
            if (LevelText != null)
                LevelText.text = $"{Mathf.FloorToInt(Player.Stats.Level)}";

            if (GoldText != null && PlayerDataManager != null)
                GoldText.text = $"{Mathf.FloorToInt(PlayerDataManager.gold)}";
        }
    }
}
