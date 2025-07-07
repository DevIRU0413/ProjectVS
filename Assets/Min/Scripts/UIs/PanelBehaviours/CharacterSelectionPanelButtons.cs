using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using ProjectVS.Manager;
using ProjectVS.Scene;
using ControlFilePanelButtonsClass = ProjectVS.UIs.PanelBehaviours.ControlFilePanelButtons.ControlFilePanelButtons;
using ProjectVS.Unit.Player;
using CharacterDescriptionTextInputManagerClass = ProjectVS.UIs.CharacterSelect.CharacterDescriptionTextInputManager.CharacterDescriptionTextInputManager;
using ProjectVS.Item;
using SelectionEffectClass = ProjectVS.UIs.CharacterSelect.SelectionEffect.SelectionEffect;
using ProjectVS.UIs.CharacterSelect.SelectionEffect;

namespace ProjectVS.UIs.PanelBehaviours.CharacterSelectionPanelButtons
{
    public class CharacterSelectionPanelButtons : MonoBehaviour
    {
        [SerializeField] private ControlFilePanelButtonsClass _controlFilePanel;
        [SerializeField] private CharacterDescriptionTextInputManagerClass _characterDisc;
        [SerializeField] private SelectionEffectClass _selectionEffectClass;

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }

        public void OnClickSelectButton()
        {
            int selectedIndex = _selectionEffectClass.CurrentIndex;
            CharacterClass selectedClass = CharacterClass.Sword; // 기본값
            PlayerStats stats;
            ItemData unique;

            switch (selectedIndex)
            {
                case 0:
                    selectedClass = CharacterClass.Sword;
                    break;
                case 1:
                    selectedClass = CharacterClass.Axe;
                    break;
                case 2:
                    selectedClass = CharacterClass.Magic;
                    break;
                default:
                    Debug.LogWarning($"[CharacterSelectionPanelButtons] 유효하지 않은 캐릭터 인덱스: {selectedIndex}");
                    return;
            }

            stats = new PlayerStats(
                1,
                selectedClass,
                _characterDisc.CharacterSelectionList[selectedIndex].HP,
                _characterDisc.CharacterSelectionList[selectedIndex].Attack,
                _characterDisc.CharacterSelectionList[selectedIndex].Defense,
                _characterDisc.CharacterSelectionList[selectedIndex].MoveSpeed,
                _characterDisc.CharacterSelectionList[selectedIndex].AttackSpeed
            );

            PlayerDataManager.Instance.Stats = stats;

            unique = ItemDatabase.Instance.GetItem(_characterDisc.CharacterSelectionList[selectedIndex].UniqueItemID);
            unique.ItemLevelUp();
            PlayerDataManager.Instance.InventoryItems.Add(unique);

            PlayerDataManager.ForceInstance.SavePlayerData(_controlFilePanel.CurrentFileIndex);

            Debug.Log($"[CharacterSelectionPanelButtons] 캐릭터 선택, 인게임씬 호출");
            SceneLoader.Instance.LoadSceneAsync(SceneID.InGameScene);
        }
    }
}
