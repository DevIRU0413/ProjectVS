using System.Collections;
using System.Collections.Generic;
using ProjectVS.Utils.UIManager;

using UnityEngine;


namespace ProjectVS.UIs.PanelBehaviours.ControlFilePanelButtons
{
    public class ControlFilePanelButtons : MonoBehaviour
    {
        public void OnClickNewButton()
        {
            //UIManager.Instance.Hide("Control File Panel");
            //UIManager.Instance.Show("File Select Panel");

            Debug.Log($"[ControlFilePanelButtons] New Button 호출");
        }

        public void OnClickLoadButton()
        {
            //UIManager.Instance.Hide("Control File Panel");
            //UIManager.Instance.Show("File Select Panel");

            Debug.Log($"[ControlFilePanelButtons] Load Button 호출");
        }

        public void OnClickDeleteButton()
        {
            //UIManager.Instance.Hide("Control File Panel");
            //UIManager.Instance.Show("File Select Panel");

            Debug.Log($"[ControlFilePanelButtons] Delete Button 호출");
        }

        public void OnClickFile1Button()
        {
            UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Show("Character Select Panel");
        }

        public void OnClickFile2Button()
        {
            UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Show("Character Select Panel");
        }

        public void OnClickFile3Button()
        {
            UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Show("Character Select Panel");
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }
    }
}
