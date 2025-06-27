using System.Collections;
using System.Collections.Generic;
using ProjectVS.Dialogue.DialogueManager;
using UnityEngine;

using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;
using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;
using EventSpriteChangeManagerClass = ProjectVS.CharacterImages.EventSpriteChangeManager.EventSpriteChangeManager;


namespace ProjectVS.CharacterImages.CostumeStateManager
{
    public class CostumeStateManager : MonoBehaviour
    {
        [SerializeField] private List<CostumeSOClass> _costumeSOs;

        public List<CostumeSOClass> CostumeSOs => _costumeSOs;

        private CostumeSOClass _currentCostume = null;

        [SerializeField] private DialogueManagerClass _dialogueManager;
        [SerializeField] private EventSpriteChangeManagerClass _eventSpriteChangeManager;

        public CostumeSOClass CurrentCostume => _currentCostume;
        public bool IsEquipped(CostumeSOClass costume) => costume.IsEquipped;

        private void Awake()
        {
            InitCostume();
        }

        // TODO: 돈이 있는지 확인하고 구매 결정해야되는 로직 추후 추가 예정
        public void ToggleOrPurchaseCostume(CostumeSOClass costume)
        {
            if (!costume.IsUnlocked)
            {
                // 안샀으면 구매처리
                costume.IsUnlocked = true;
                Debug.Log($"[CostumeStateManager] {costume.CostumeName} 구매 완료");
            }
            else if (costume.IsEquipped)
            {
                // 착용 중이면 해제
                costume.IsEquipped = false;
                _currentCostume = null;
                _eventSpriteChangeManager.ChangeRepeatImage(_dialogueManager.CurrentDialogueData.IllustPath);
                _dialogueManager.ShowRepeatDialogue();
                Debug.Log($"[CostumeStateManager] {costume.CostumeName} 미착용 상태로 전환");
            }
            else
            {
                // 착용
                EquipCostume(costume);
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

            _dialogueManager.ShowBuyDialogue(costume.CostumeDialogueID);
            _eventSpriteChangeManager.ChangeCostumeImage();
        }


        // MEMO: 현재는 Awake에서 ScriptableObject 초기화 하고 있음
        // 다른 씬을 왕복할 시 초기화 되는 구조
        // TODO: 추후 씬 전환 시 CostumeStateManager를 유지하도록 InitCostume 호출하지 않도록 변경
        private void InitCostume()
        {
            foreach (var costume in _costumeSOs)
            {
                costume.IsEquipped = false;
                costume.IsUnlocked = false;
            }
        }
    }
}
