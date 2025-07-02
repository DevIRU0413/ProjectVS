using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CostumeStateManagerClass = ProjectVS.CharacterImages.CostumeStateManager.CostumeStateManager;


namespace ProjectVS.CharacterImages.EventSpriteChangeManager
{
    public class EventSpriteChangeManager : MonoBehaviour
    {
        private Dictionary<string, Sprite> _spriteDict = new();

        [SerializeField] private Image _heroinEventImage;
        [SerializeField] private Image _shopNPCEvnetImage;
        [SerializeField] private Image _cutSceneImage;
        [SerializeField] private Image _repeatImage;

        [SerializeField] private CostumeStateManagerClass _costumeStateManager;

        private Color _disabledColor;
        private Color _enabledColor;

        private void Awake()
        {
            InitColor();
        }

        public void ChangeEventImage(string path, int characterID)
        {
            if (!_spriteDict.TryGetValue(path, out Sprite sprite))
            {
                sprite = Resources.Load<Sprite>(path);
                if (sprite == null)
                {
                    Debug.LogWarning($"[SpriteChangeManager] ChangeEventImage: 경로{path}를 찾을 수 없습니다.");
                    return;
                }
                else
                {
                    _spriteDict[path] = sprite;
                    Debug.Log($"[SpriteChangeManager] ChangeEventImage: 경로{path}를 새로 로드했습니다.");
                }
            }

            if (characterID == 1) // 주인공
            {
                _cutSceneImage.enabled = false;
                _heroinEventImage.color = _enabledColor;
                _heroinEventImage.sprite = sprite;
                _shopNPCEvnetImage.color = _disabledColor;
            }
            else if (characterID == 2)// 상점 NPC
            {
                _cutSceneImage.enabled = false;
                _shopNPCEvnetImage.color = _enabledColor;
                _shopNPCEvnetImage.sprite = sprite;
                _heroinEventImage.color = _disabledColor;
            }
            else // 3번은 배경 이미지
            {
                _cutSceneImage.enabled = true;
                _cutSceneImage.sprite = sprite;
            }

            Debug.Log($"[SpriteChangeManager] ChangeEventImage: 이미지 변경 완료. 경로: {path}");
            Debug.Log($"[SpriteChangeManager] ChangeEventImage: 현재 스프라이트 이름: {_heroinEventImage.sprite?.name}");
        }


        public void ChangeRepeatImage(string path)
        {
            // 코스튬을 장착 중이라면 기존 스프라이트 무시하고 스프라이트 코스튬 스프라이트 사용
            if (_costumeStateManager.CurrentCostume != null) return;

            if (!_spriteDict.TryGetValue(path, out Sprite sprite))
            {
                sprite = Resources.Load<Sprite>(path);
                if (sprite == null)
                {
                    Debug.LogWarning($"[SpriteChangeManager] ChangeRepeatImage: 경로{path}를 찾을 수 없습니다.");
                    return;
                }
                else
                {
                    _spriteDict[path] = sprite;
                    Debug.Log($"[SpriteChangeManager] ChangeRepeatImage: 경로{path}를 새로 로드했습니다.");
                }
            }

            _repeatImage.sprite = sprite;
            Debug.Log($"[SpriteChangeManager] ChangeRepeatImage: 이미지 변경 완료. 경로: {path}");
            Debug.Log($"[SpriteChangeManager] ChangeRepeatImage: 현재 스프라이트 이름: {_repeatImage.sprite?.name}");
        }

        public void ChangeCostumeImage()
        {
            if (_costumeStateManager.CurrentCostume != null)
            {
                _repeatImage.sprite = _costumeStateManager.CurrentCostume.CostumeSprite;
                Debug.Log($"[SpriteChangeManager] ChangeRepeatImage: 코스튬 스프라이트 사용: {_repeatImage.sprite.name}");
                return;
            }
        }

        private void InitColor()
        {
            _disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            _enabledColor = new Color(1f, 1f, 1f, 1f);
        }
    }
}
