using System.Collections;
using System.Collections.Generic;

using ProjectVS.Utils.UIManager;

using UnityEngine;


namespace ProjectVS.UIs.PanelBehaviours.FileSelectPanelButtons
{
    public class FileSelectPanelButtons : MonoBehaviour
    {
        public void OnClickFile1Button()
        {
            UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Hide("File Select Panel");
            UIManager.Instance.Show("Character Select Panel");
        }

        public void OnClickFile2Button()
        {
            UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Hide("File Select Panel");
            UIManager.Instance.Show("Character Select Panel");
        }

        public void OnClickFile3Button()
        {
            UIManager.Instance.Hide("Control File Panel");
            UIManager.Instance.Hide("File Select Panel");
            UIManager.Instance.Show("Character Select Panel");
        }
    }
}
