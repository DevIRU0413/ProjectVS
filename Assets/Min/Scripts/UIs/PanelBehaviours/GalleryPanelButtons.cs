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

            CheckLocked();
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

                    ShowThumbnail(i);
                }
                else
                {
                    _lockedImages[i].enabled = true;
                    _buttons[i].interactable = false;
                }
            }
        }

        private void ShowThumbnail(int index)
        {
            Image buttonImage = _buttons[index].GetComponent<Image>();
            if (buttonImage != null && _images[index].sprite != null)
            {
                Sprite fullSprite = _images[index].sprite;
                Texture2D texture = fullSprite.texture;

                // 원본 스프라이트의 중심 정사각형 영역 계산
                Rect originalRect = fullSprite.textureRect;
                float squareSize = Mathf.Min(originalRect.width, originalRect.height);
                float x = originalRect.x + (originalRect.width - squareSize) / 2f;
                float y = originalRect.y + (originalRect.height - squareSize) / 2f;
                Rect squareRect = new Rect(x, y, squareSize, squareSize);

                // 새 스프라이트 생성 (pivot은 가운데로)
                Sprite croppedSprite = Sprite.Create(
                    texture,
                    squareRect,
                    new Vector2(0.5f, 0.5f),
                    fullSprite.pixelsPerUnit
                );

                buttonImage.sprite = croppedSprite;
                buttonImage.color = Color.white;
            }
        }
    }
}
