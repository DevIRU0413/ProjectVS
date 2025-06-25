using System.Collections;
using System.Collections.Generic;

using ProjectVS.Utils.UIManager;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.UIs.PanelBehaviours.GalleryPanelButtons
{
    public class GalleryPanelButtons : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _images;
        [SerializeField] private List<Button> _buttons;

        private void Start()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                int index = i;
                _buttons[i].onClick.AddListener(() => ShowImage(index));
            }
        }

        private void ShowImage(int index)
        {
            for (int i = 0; i < _images.Count; i++)
            {
                if (i == index)
                {
                    GameObject panel = _images[i];
                    UIManager.Instance.Show(panel); // 오버로딩된 Show(GameObject) 사용
                }
                else
                {
                    _images[i].SetActive(false);
                }
            }
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }
    }
}
