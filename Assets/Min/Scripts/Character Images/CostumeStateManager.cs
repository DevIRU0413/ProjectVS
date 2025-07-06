using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Dialogue.DialogueManager;
using ProjectVS.Manager;
using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;
using EventSpriteChangeManagerClass = ProjectVS.CharacterImages.EventSpriteChangeManager.EventSpriteChangeManager;
using ShopSceneNPCBehaviourClass = ProjectVS.NPC.ShopSceneNPCBehaviour.ShopSceneNPCBehaviour;
using ProjectVS.NPC.ShopSceneNPCBehaviour;
using ProjectVS.UIs.CostumeBuyButton;
using ProjectVS.Dialogue.DialogueManagerR;


namespace ProjectVS.CharacterImages.CostumeStateManager
{
    // TODO: PlayerData 연동 후 세이브 관련 주석 해제
    public class CostumeStateManager : MonoBehaviour
    {
        [SerializeField] private List<CostumeSOClass> _costumeSOs;

        public List<CostumeSOClass> CostumeSOs => _costumeSOs;

        private CostumeSOClass _currentCostume = null;
        private CostumeSOClass _wantToBuyCostume = null;
        protected CostumeBuyButton _buyButton;

        [SerializeField] private EventSpriteChangeManagerClass _eventSpriteChangeManager;
        [SerializeField] private ShopSceneNPCBehaviourClass _shopSceneNPCBehaviour;


        public CostumeSOClass CurrentCostume => _currentCostume;
        public CostumeSOClass WantToBuyCostume => _wantToBuyCostume;
        public CostumeBuyButton CostumeBuyButton => _buyButton;

        public bool IsEquipped(CostumeSOClass costume) => costume.IsEquipped;

        private void Awake()
        {
            //LoadAcquiredCostumes();
            //LoadWornCostume();
        }

        // TODO: 돈이 있는지 확인하고 구매 결정해야되는 로직 추후 추가 예정
        public void ToggleOrPurchaseCostume(CostumeSOClass costume)
        {
            if (!costume.IsUnlocked)
            {
                // 안샀으면 구매처리
                costume.IsUnlocked = true;
                //SaveAcquiredCostumes();
                Debug.Log($"[CostumeStateManager] {costume.CostumeName} 구매 완료");
            }
            else if (costume.IsEquipped)
            {
                // 착용 중이면 해제
                costume.IsEquipped = false;
                _currentCostume = null;
                //_eventSpriteChangeManager.ChangeRepeatImage(DialogueManager.Instance.CurrentDialogueData.IllustPath);
                //DialogueManager.Instance.ShowRepeatDialogue();

                _eventSpriteChangeManager.ChangeRepeatImage(DialogueManagerR.Instance.CurrentDialogueData.IllustPath);
                DialogueManagerR.Instance.CanShowDialogueByType(DialogueType.Repeat);

                //SaveWornCostume();
                _shopSceneNPCBehaviour.RenewCostumeAnimation();
                Debug.Log($"[CostumeStateManager] {costume.CostumeName} 미착용 상태로 전환");
            }
            else
            {
                // 착용
                EquipCostume(costume);
                //SaveWornCostume();
                _shopSceneNPCBehaviour.RenewCostumeAnimation();
                Debug.Log($"[CostumeStateManager] {costume.CostumeName} 착용 완료");
            }
        }

        private void EquipCostume(CostumeSOClass costume)
        {
            // 다른 코스튬 자동 장착 해제
            foreach (var other in _costumeSOs)
            {
                other.IsEquipped = false;
            }

            costume.IsEquipped = true;
            _currentCostume = costume;

            //DialogueManager.Instance.ShowBuyDialogue(costume.CostumeDialogueID);
            DialogueManagerR.Instance.ShowDialogueByType(DialogueType.WearCostume);
            _eventSpriteChangeManager.ChangeCostumeImage();
        }


        public void SaveAcquiredCostumes()
        {
            HashSet<string> acquiredCostumes = new();
            foreach (var costume in _costumeSOs)
            {
                if (costume.IsUnlocked)
                {
                    acquiredCostumes.Add(costume.name);
                }
            }

            PlayerDataManager.Instance.AcquiredCostumeName = acquiredCostumes;
        }

        public void SaveWornCostume()
        {
            foreach (var costume in _costumeSOs)
            {
                if (costume.IsEquipped)
                {
                    PlayerDataManager.Instance.WornCostumeName = costume.name;
                }
            }
        }

        public void WillBuyCostume(CostumeSOClass costume, CostumeBuyButton button)
        {
            _wantToBuyCostume = costume;
            _buyButton = button;
        }

        private void LoadAcquiredCostumes()
        {
            HashSet<string> acquiredCostumes = PlayerDataManager.Instance.AcquiredCostumeName;

            if (acquiredCostumes == null || acquiredCostumes.Count == 0)
            {
                Debug.Log("[CostumeStateManager] 획득한 코스튬이 없습니다.");
                return;
            }

            foreach (var costume in _costumeSOs)
            {
                if (acquiredCostumes.Contains(costume.name))
                {
                    costume.IsUnlocked = true;
                }
                else
                {
                    costume.IsUnlocked = false;
                }
            }
        }

        private void LoadWornCostume()
        {
            if (string.IsNullOrEmpty(PlayerDataManager.Instance.WornCostumeName))
            {
                Debug.Log("[CostumeStateManager] 착용 중인 코스튬이 없습니다.");
                return;
            }

            foreach (var costume in _costumeSOs)
            {
                if (costume.name == PlayerDataManager.Instance.WornCostumeName)
                {
                    costume.IsEquipped = true;
                    _currentCostume = costume;
                    _eventSpriteChangeManager.ChangeCostumeImage();
                    return;
                }
                else
                {
                    costume.IsEquipped = false;
                }
            }
        }
    }
}
