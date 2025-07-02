using ProjectVS;
using ProjectVS.Data;
using ProjectVS.Manager;

using TMPro;

using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;
using UnityEngine.UI;
namespace ProjectVS.JDW
{ 
    // 추후 Ui관련 팀원분과 조율 예정
    public class BattleSceneUI : MonoBehaviour
    {
        [Header("Bar Ui")]
        public PixelUI.ValueBar ExpBar;    //경험치바를 채울 이미지
        public PixelUI.ValueBar BossHpBar; //보스 체력바를 채울 이미지

       [HideInInspector] public PlayerConfig Player;
       [HideInInspector] public Boss Boss;
       [HideInInspector] public PlayerDataManager PlayerDataManager;

        [Header("References")]
        public TextMeshProUGUI LevelText;   // 레벨 
        public TextMeshProUGUI GoldText;    // 골드
        public TextMeshProUGUI DiamondsText;// 다이아

        [Header("Score UI")]
        public GameObject ScoreUi;
        public TextMeshProUGUI MonsterScore; // 몬스터 처치 수
        public TextMeshProUGUI PlayerTime;   // 총 플레이 타임
        public TextMeshProUGUI Stage;        // 총 스테이지

        [Header("Player Portirait")]
        [SerializeField] private Sprite[] _hpPortraits;
        [SerializeField] private Image _portraitImage;
        private void Awake()
        {
            Player = FindObjectOfType<PlayerConfig>();
            Boss = FindObjectOfType<Boss>();
        }
        private void Start()
        {
            //초기에 플레이어가 없을 경우 대체용
            if (ExpBar != null)
            {
                ExpBar.CurrentValue = 0f;
                ExpBar.MaxValue = 1f;
            }
        }

        private void Update()
        {
           if(Player == null || Boss == null) // 플레이어와 보스가 없을 경우 
            {
                TryFindReferences();
            }
            if (Player == null || Player.Stats == null)
                return;

            UpdateExpBar();
            UpdateBossHpBar();
            UpdateTexts();
        }
        private void TryFindReferences()
        {
            if (Player == null)
                Player = FindObjectOfType<PlayerConfig>(); // 자동으로 플레이어 찾음       
            if (Boss == null)
                Boss = FindObjectOfType<Boss>(); // 자동으로 보스를 찾음
        }
        public void UpdatePortrait(float hpRatio)
        {

            hpRatio = Mathf.Clamp01(hpRatio); 

            if (hpRatio > 0.8f) // 체력 비율에 맞춰 이미지 변화
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
            if (Player == null || Player.Stats == null) 
                return;

            if (LevelText != null)
                LevelText.text = $"{Mathf.FloorToInt(Player.Stats.Level)}";

            if (PlayerDataManager.Instance != null)
            {
                if (GoldText != null && PlayerDataManager.Instance != null)
                    GoldText.text = $"{Mathf.FloorToInt(PlayerDataManager.Instance.gold)}";
                if (DiamondsText != null && PlayerDataManager.Instance != null)
                    DiamondsText.text = $"{Mathf.FloorToInt(PlayerDataManager.Instance.diamonds)}";
            }
        }
        public void ShowDeathResult()
        {
            var data = PlayerDataManager.Instance;

            int min = (int)(data.totalPlayTime / 60f);
            int sec = (int)(data.totalPlayTime % 60f);

            MonsterScore.text = $"MonsterScore : {data.totalKills}";
            PlayerTime.text = $"PlayerTime: {min:D2}:{sec:D2}";
            Stage.text = $"Total Stage: {data.battleSceneCount}";

            ScoreUi.SetActive(true);
        }
    }
}
