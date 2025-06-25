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
            UIManager.Instance.Show("File Select Panel");
        }

        public void OnClickLoadButton()
        {
            //UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Show("File Select Panel");
        }

        public void OnClickDeleteButton()
        {
            //UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Show("File Select Panel");
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }
    }
}
