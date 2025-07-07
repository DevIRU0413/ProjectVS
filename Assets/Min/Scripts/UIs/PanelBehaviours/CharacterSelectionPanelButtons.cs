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

namespace ProjectVS.UIs.PanelBehaviours.CharacterSelectionPanelButtons
{
    public class CharacterSelectionPanelButtons : MonoBehaviour
    {
        [SerializeField] private ControlFilePanelButtonsClass _controlFilePanel;
        [SerializeField] private CharacterDescriptionTextInputManagerClass _characterDisc;


        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }

        public void OnClickSelectButton()
        {
            switch (_controlFilePanel.CurrentFileIndex)
            {
                case 1:
                    PlayerDataManager.Instance.Stats.CharacterClass = CharacterClass.Sword;
                    PlayerStats p1Stat = new(
                        1,
                        CharacterClass.Sword,
                        _characterDisc.CharacterSelectionList[0].HP,
                        _characterDisc.CharacterSelectionList[0].Attack,
                        _characterDisc.CharacterSelectionList[0].Defense,
                        _characterDisc.CharacterSelectionList[0].MoveSpeed,
                        _characterDisc.CharacterSelectionList[0].AttackSpeed
                        );
                    PlayerDataManager.Instance.Stats = p1Stat;

                    ItemData unique1 = ItemDatabase.Instance.GetItem(_characterDisc.CharacterSelectionList[0].UniqueItemID);
                    unique1.ItemLevelUp();
                    PlayerDataManager.Instance.InventoryItems.Add(unique1);

                    PlayerDataManager.ForceInstance.SavePlayerData(_controlFilePanel.CurrentFileIndex);
                    break;
                case 2:
                    PlayerDataManager.Instance.Stats.CharacterClass = CharacterClass.Axe;
                    PlayerStats p2Stat = new(
                        2,
                        CharacterClass.Axe,
                        _characterDisc.CharacterSelectionList[1].HP,
                        _characterDisc.CharacterSelectionList[1].Attack,
                        _characterDisc.CharacterSelectionList[1].Defense,
                        _characterDisc.CharacterSelectionList[1].MoveSpeed,
                        _characterDisc.CharacterSelectionList[1].AttackSpeed
                        );
                    PlayerDataManager.Instance.Stats = p2Stat;

                    ItemData unique2 = ItemDatabase.Instance.GetItem(_characterDisc.CharacterSelectionList[1].UniqueItemID);
                    unique2.ItemLevelUp();
                    PlayerDataManager.Instance.InventoryItems.Add(unique2);

                    PlayerDataManager.ForceInstance.SavePlayerData(_controlFilePanel.CurrentFileIndex);
                    break;
                case 3:
                    PlayerDataManager.Instance.Stats.CharacterClass = CharacterClass.Magic;
                    PlayerStats p3Stat = new(
                        3,
                        CharacterClass.Magic,
                        _characterDisc.CharacterSelectionList[2].HP,
                        _characterDisc.CharacterSelectionList[2].Attack,
                        _characterDisc.CharacterSelectionList[2].Defense,
                        _characterDisc.CharacterSelectionList[2].MoveSpeed,
                        _characterDisc.CharacterSelectionList[2].AttackSpeed
                        );
                    PlayerDataManager.Instance.Stats = p3Stat;

                    ItemData unique3 = ItemDatabase.Instance.GetItem(_characterDisc.CharacterSelectionList[2].UniqueItemID);
                    unique3.ItemLevelUp();
                    PlayerDataManager.Instance.InventoryItems.Add(unique3);

                    PlayerDataManager.ForceInstance.SavePlayerData(_controlFilePanel.CurrentFileIndex);
                    break;
                default:
                    Debug.LogWarning($"[CharacterSelectionPanelButtons] 유효하지 않은 파일 인덱스: {_controlFilePanel.CurrentFileIndex}");
                    break;
            }

            Debug.Log($"[CharacterSelectionPanelButtons] 캐릭터 선택, 인게임씬 호출");
            SceneLoader.Instance.LoadSceneAsync(SceneID.InGameScene);
        }
    }
}
