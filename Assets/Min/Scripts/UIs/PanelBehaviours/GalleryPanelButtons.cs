using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using ProjectVS.Utils.UIManager;
using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;


namespace ProjectVS.UIs.PanelBehaviours.GalleryPanelButtons
{
    public class GalleryPanelButtons : MonoBehaviour
    {
        [SerializeField] private List<Image> _images; // 확대해서 보여줄 이미지
        [SerializeField] private List<Button> _buttons;
        private List<Image> _lockedImages = new();
        [SerializeField] private List<CostumeSOClass> costumeSOs;


        private void Awake()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                int index = i;
                _buttons[i].onClick.AddListener(() => ShowImage(index));

                Image image = _buttons[i].GetComponentsInChildren<Image>(true).FirstOrDefault(img => img.gameObject != _buttons[i].gameObject);
                _lockedImages.Add(image);
            }
        }

        private void OnEnable()
        {
            CheckLocked();
        }

        private void Start()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                if (_images[i] == null)
                {
                    _images[i] = GetComponentsInChildren<Image>(true).Where(img => img.gameObject.name == $"Image {i}").FirstOrDefault();
                }

                _images[i].sprite = costumeSOs[i].FullScene;
            }
        }

        private void ShowImage(int index)
        {
            for (int i = 0; i < _images.Count; i++)
            {
                if (i == index)
                {
                    GameObject panel = _images[i].gameObject;
                    UIManager.Instance.Show(panel); // 오버로딩된 Show(GameObject) 사용
                }
                else
                {
                    _images[i].gameObject.SetActive(false);
                }
            }
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }

        private void CheckLocked()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (costumeSOs[i].IsUnlocked)
                {
                    _lockedImages[i].enabled = false;
                    _buttons[i].interactable = true;
                }
                else
                {
                    _lockedImages[i].enabled = true;
                    _buttons[i].interactable = false;
                }
            }
        }
    }
}
