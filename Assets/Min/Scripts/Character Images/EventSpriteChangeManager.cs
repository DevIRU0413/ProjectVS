using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.CharacterImages.SpriteChangeManager
{
    public class EventSpriteChangeManager : MonoBehaviour
    {
        private Dictionary<string, Sprite> _spriteDict = new();

        [SerializeField] private Image _heroinEventImage;
        [SerializeField] private Image _shopNPCEvnetImage;
        [SerializeField] private Image _repeatImage;

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
                _heroinEventImage.color = _enabledColor;
                _heroinEventImage.sprite = sprite;
                _shopNPCEvnetImage.color = _disabledColor;
            }
            else // 상점 NPC or 기타 인물
            {
                _shopNPCEvnetImage.color = _enabledColor;
                _shopNPCEvnetImage.sprite = sprite;
                _heroinEventImage.color = _disabledColor;
            }

            Debug.Log($"[SpriteChangeManager] ChangeEventImage: 이미지 변경 완료. 경로: {path}");
            Debug.Log($"[SpriteChangeManager] ChangeEventImage: 현재 스프라이트 이름: {_heroinEventImage.sprite?.name}");
        }


        public void ChangeRepeatImage(string path)
        {
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

        private void InitColor()
        {
            _disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            _enabledColor = new Color(1f, 1f, 1f, 1f);
        }
    }
}
