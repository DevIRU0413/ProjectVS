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
            Debug.Log($"[CharacterSelectionPanelButtons] 캐릭터 선택, 인게임씬 호출");
            SceneLoader.Instance.LoadSceneAsync(SceneID.InGameScene);
        }
    }
}
