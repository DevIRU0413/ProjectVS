using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using ProjectVS.Manager;
using ProjectVS.Scene;


namespace ProjectVS.UIs.PanelBehaviours.CharacterSelectionPanelButtons
{
    public class CharacterSelectionPanelButtons : MonoBehaviour
    {
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
                    PlayerDataManager.ForceInstance.SavePlayerData(_controlFilePanel.CurrentFileIndex);
                    break;
                case 2:
                    PlayerDataManager.Instance.Stats.CharacterClass = CharacterClass.Axe;
                    PlayerDataManager.ForceInstance.SavePlayerData(_controlFilePanel.CurrentFileIndex);
                    break;
                case 3:
                    PlayerDataManager.Instance.Stats.CharacterClass = CharacterClass.Magic;
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
