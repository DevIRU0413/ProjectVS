using System.Collections;
using System.Collections.Generic;

using ProjectVS.Utils.UIManager;

using UnityEngine;

using NPCBehaviourClass = ProjectVS.NPC.NPCBehaviour.FieldNPCBehaviour;

namespace ProjectVS.UIs.PanelBehaviours.NPCInteractionSelectPanelButtons
{
    public class NPCInteractionSelectPanelButtons : MonoBehaviour
    {
        [SerializeField] private NPCBehaviourClass _npcBehaviour;

        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void OnClickSaveButton()
        {
            _npcBehaviour.Save();
            UIManager.Instance.ForceCloseTopPanel();
        }

        public void OnClickRobButton()
        {
            _npcBehaviour.Rob();
            UIManager.Instance.ForceCloseTopPanel();
        }
    }
}
