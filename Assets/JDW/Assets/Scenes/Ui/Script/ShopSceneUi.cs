using ProjectVS;
using ProjectVS.Data;
using ProjectVS.Manager;

using TMPro;

using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;
using UnityEngine.UI;
namespace ProjectVS.JDW
{
    public class ShopSceneUi : MonoBehaviour
    {
        [HideInInspector] public PlayerConfig Player;

        [Header("References")]
        public TextMeshProUGUI LevelText;   // 레벨 
        public TextMeshProUGUI GoldText;    // 골드
        public TextMeshProUGUI DiamondsText;// 다이아
        private void Awake()
        {
            Player = FindObjectOfType<PlayerConfig>();
        }
        private void Update()
        {
            if (Player == null) // 플레이어와 보스가 없을 경우 
            {
                TryFindReferences();
            }
            if (Player == null || Player.Stats == null)
                return;

            UpdateTexts();
        }
        private void TryFindReferences()
        {
            if (Player == null)
                Player = FindObjectOfType<PlayerConfig>(); // 자동으로 플레이어 찾음       
        }
        private void UpdateTexts()
        {
            if (LevelText != null)
                LevelText.text = $"{Mathf.FloorToInt(Player.Stats.Level)}";

            if (PlayerDataManager.Instance != null)
            {
                if (GoldText != null && PlayerDataManager.Instance != null)
                    GoldText.text = $"{Mathf.FloorToInt(PlayerDataManager.Instance.Gold)}";
                if (DiamondsText != null && PlayerDataManager.Instance != null)
                    DiamondsText.text = $"{Mathf.FloorToInt(PlayerDataManager.Instance.Diamonds)}";
            }
        }
    }
    
}
